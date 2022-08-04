using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Data.Identity.DbContext;
using Data.Constants;
using Microsoft.EntityFrameworkCore;
using WebRazor.ViewModels.Billing;
using Data.Identity.Models;
using Cayent.Core.Common.Extensions;
using System.Net.Http.Headers;
using Data.Identity.Models.Billings;
using App.Services;
using System.Text;
using Newtonsoft.Json;
using Data.Identity.Models.Gcash;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using CsvHelper;
using System.Globalization;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Data.Identity.Models.Readings;

namespace WebRazor.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ReaderController : BaseController
    {
        private IWebHostEnvironment _environment;
        IdentityWebContext _identityWebContext;
        readonly NotificationService _notificationService;
        readonly IConfiguration _configuration;

        public ReaderController(
            IdentityWebContext identityWebContext, NotificationService notificationService, IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            _identityWebContext = identityWebContext;
            _notificationService = notificationService;
            _configuration = configuration;
            _environment = environment;
        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage(CancellationToken cancellationToken)
        {
            var imgFile = HttpContext.Request.Form.Files.FirstOrDefault();

            if (imgFile == null)
                return BadRequest("No file uploaded.");

            var items = await GetValues(imgFile.OpenReadStream());

            var items2 = items.Where(e => e.Length >= 5 || e.Length == 12).ToList();

            var meterNumber = items2.Where(e => e.Length == 12).FirstOrDefault();

            if (string.IsNullOrWhiteSpace(meterNumber))
            {
                return BadRequest("Meter Number not found in the uploaded image.");
            }

            var account = await _identityWebContext
                .Accounts
                .Include(e => e.UserInformation)
                .FirstAsync(e => e.AccountId == UserId);
            var user = await _identityWebContext.Users.FirstAsync(e => e.Id == UserId);

            if (meterNumber != account.MeterNumber)
            {
                return BadRequest("Meter Number is invalid.");
            }

            var dto = new
            {
                Items = items2.Where(e => e != meterNumber),
                account.AccountNumber,
                account.MeterNumber,
                user.PhoneNumber,
                user.UserName,
                Customer = $"{account.UserInformation.FirstName} {account.UserInformation.LastName}"

            };

            return Ok(dto);
        }

        [HttpPost("add-reading/{value}")]
        public async Task<IActionResult> AddReading(double value,
            [FromServices] IdentityWebContext dbContext,
            CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow.Truncate();

            var data = new MeterReading
            {
                AccountId = UserId,
                Value = value,
                DateRead = now,
            };

            await dbContext.MeterReadings.AddAsync(data, cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);

            return Ok();
        }

        [HttpGet("get-read/{accountId}")]
        public async Task<IActionResult> GetMeterReadings(string accountId,
            [FromServices] IdentityWebContext dbContext,
            CancellationToken cancellationToken)
        {
            var meterReadings = await dbContext
                .MeterReadings.Where(e => e.AccountId == accountId)
                .OrderByDescending(e => e.DateRead)
                .Take(7)
                .ToListAsync();

            var lastBilling = await dbContext.Billings
                .Where(e => e.AccountId == accountId)
                .OrderByDescending(e => e.DateEnd)
                .FirstOrDefaultAsync();

            var dto = new
            {
                PreviousRead = lastBilling?.PresentReading,
                MeterReadings = meterReadings
            };

            return Ok(dto);
        }

        [HttpGet("get-billing-readings/{accountId}")]
        public async Task<IActionResult> GetPreviousReadings(string accountId,
            [FromServices] IdentityWebContext dbContext,
            CancellationToken cancellationToken)
        {
            var readings = await dbContext.Billings
                .Where(e => e.AccountId == accountId)
                .OrderByDescending(e => e.DateEnd)
                .Select(e => e.PresentReading)
                .ToListAsync();

            return Ok(readings);
        }

        async Task<IEnumerable<string>> GetValues(Stream fileStream)
        {
            string Key = "facee84a442a408c86c020dbbb91d9f0";
            // put your Cognitive Service URL
            string url = "https://caydev-cv-service.cognitiveservices.azure.com/";


            var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(Key));
            client.Endpoint = url;

            var textHeaders = await client.ReadInStreamAsync(fileStream);
            string operationLocation = textHeaders.OperationLocation;

            Thread.Sleep(2000);

            // Retrieve the URI where the extracted text will be stored from the Operation-Location header.
            // We only need the ID and not the full URL
            const int numberOfCharsInOperationId = 36;
            string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

            // Extract the text
            ReadOperationResult results;
            Console.WriteLine($"Extracting text from URL file...");
            Console.WriteLine();
            do
            {
                results = await client.GetReadResultAsync(System.Guid.Parse(operationId));
            }
            while ((results.Status == OperationStatusCodes.Running ||
                results.Status == OperationStatusCodes.NotStarted));

            // Display the found text.            
            var textUrlFileResults = results.AnalyzeResult.ReadResults;

            var possibleValues = new List<string>();

            foreach (ReadResult page in textUrlFileResults)
            {
                foreach (Line line in page.Lines)
                {
                    var texts = line.Text.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var text in texts)
                    {
                        var isDigit = text.All(e => char.IsDigit(e));

                        if (isDigit)
                            possibleValues.Add(text);
                    }
                }
            }

            return possibleValues;
        }
    }
}
