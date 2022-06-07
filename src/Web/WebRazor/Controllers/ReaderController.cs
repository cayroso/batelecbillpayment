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

namespace WebRazor.Controllers
{
    [ApiController]
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

            return Ok(items);
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
