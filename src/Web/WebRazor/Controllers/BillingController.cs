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

namespace WebRazor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class BillingController : BaseController
    {
        IdentityWebContext _identityWebContext;
        readonly NotificationService _notificationService;
        readonly IConfiguration _configuration;

        public BillingController(IdentityWebContext identityWebContext, NotificationService notificationService, IConfiguration configuration)
        {
            _identityWebContext = identityWebContext;
            _notificationService = notificationService;
            _configuration = configuration;
        }

        [HttpGet("{billingId}")]
        public async Task<IActionResult> Get(string billingId, CancellationToken cancellationToken)
        {
            var isAdmin = User.IsInRole(ApplicationRoles.AdministratorRoleName);

            var dto = await _identityWebContext.Billings
                .AsNoTracking()
                .Where(e => (e.AccountId == UserId || isAdmin) && e.BillingId == billingId)
                .Select(e => new ViewBillingInfo
                {
                    AccountName = e.Account.UserInformation.FirstLastName,
                    AccountNumber = e.Account.AccountNumber,

                    GCashSourceResourceId = e.GcashResource!.GcashResourceId,
                    GCashCheckoutUrl = e.GcashResource!.CheckoutUrl,
                    DateEnd = e.DateEnd,
                    DateStart = e.DateStart,
                    DateDue = e.DateDue,
                    Amount = e.Amount,
                    BillingId = e.BillingId,
                    Status = (int)e.Status,
                    StatusText = e.Status.ToString(),
                    Month = e.Month,
                    Number = e.Number,
                    Year = e.Year,
                    KilloWattHourUsed = e.KilloWattHourUsed,
                    Multiplier = e.Multiplier,
                    PresentReading = e.PresentReading,
                    PreviousReading = e.PreviousReading,
                    Reader = e.Reader,
                    ReadingDate = e.ReadingDate,
                    Token = e.ConcurrencyToken,
                    Resource = new WebRazor.ViewModels.Billing.SourceResource
                    {
                        Amount = e.GcashResource == null ? 0 : e.GcashResource.Amount / 100,
                        CheckoutUrl = e.GcashResource == null ? "" : e.GcashResource.CheckoutUrl,
                        //NetAmount = e.GcashResource == null ? 0 : e.GcashResource.net / 100,
                    },
                    Payment = new PaymentResource
                    {
                        GcashPaymentId = e.GcashPayment == null ? "" : e.GcashPayment.GcashPaymentId,
                        Amount = e.GcashPayment == null ? 0 : e.GcashPayment.Amount / 100,
                        Description = e.GcashPayment == null ? "" : e.GcashPayment.Description,
                        Fee = e.GcashPayment == null ? 0 : e.GcashPayment.Fee / 100,
                        NetAmount = e.GcashPayment == null ? 0 : e.GcashPayment!.NetAmount / 100,
                    }
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (dto == null)
                return BadRequest("Billing not found.");

            //if(!string.IsNullOrWhiteSpace(dto.GCashSourceResourceId))
            //{
            //    //  check if not expired
            //    var gcashResource = await GetGcashResource(dto.GCashSourceResourceId, cancellationToken);

            //    if(gcashResource.Data.Attributes.Status != "dddd")
            //    {

            //    }
            //}



            return Ok(dto);
        }

        [HttpGet]

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Billings(string c, int p, int s, string sf, int so, CancellationToken cancellationToken)
        {
            var sql = from e in _identityWebContext.Billings.AsNoTracking()

                      where string.IsNullOrWhiteSpace(c)
                            || EF.Functions.Like(e.Account.AccountNumber, $"%{c}%")
                            || EF.Functions.Like(e.Account.MeterNumber, $"%{c}%")
                      //|| EF.Functions.Like(e.LastName, $"%{c}%")

                      select new ViewBillingInfo
                      {
                          GCashSourceResourceId = e.GcashResource!.GcashResourceId,
                          AccountName = e.Account.UserInformation.FirstLastName,
                          AccountNumber = e.Account.AccountNumber,
                          Status = (int)e.Status,

                          GCashCheckoutUrl = e.GcashResource!.CheckoutUrl,
                          DateEnd = e.DateEnd,
                          DateStart = e.DateStart,
                          DateDue = e.DateDue,
                          Amount = e.Amount,
                          BillingId = e.BillingId,
                          StatusText = e.Status.ToString(),
                          Month = e.Month,
                          Number = e.Number,
                          Year = e.Year,
                          KilloWattHourUsed = e.KilloWattHourUsed,
                          Multiplier = e.Multiplier,
                          PresentReading = e.PresentReading,
                          PreviousReading = e.PreviousReading,
                          Reader = e.Reader,
                          ReadingDate = e.ReadingDate,
                          Token = e.ConcurrencyToken,
                          Resource = new WebRazor.ViewModels.Billing.SourceResource
                          {
                              Amount = e.GcashResource == null ? 0 : e.GcashResource.Amount / 100,
                              CheckoutUrl = e.GcashResource == null ? "" : e.GcashResource.CheckoutUrl,
                              //NetAmount = e.GcashResource == null ? 0 : e.GcashResource.net / 100,
                          },
                          Payment = new PaymentResource
                          {
                              GcashPaymentId = e.GcashPayment == null ? "" : e.GcashPayment.GcashPaymentId,
                              Amount = e.GcashPayment == null ? 0 : e.GcashPayment.Amount / 100,
                              Description = e.GcashPayment == null ? "" : e.GcashPayment.Description,
                              Fee = e.GcashPayment == null ? 0 : e.GcashPayment.Fee / 100,
                              NetAmount = e.GcashPayment == null ? 0 : e.GcashPayment!.NetAmount / 100,
                          }

                      };

            var dto = await sql.ToPagedItemsAsync(p, s, cancellationToken);

            return Ok(dto);
        }

        [HttpGet("my-billings")]
        public async Task<IActionResult> MyBillings(string c, int p, int s, string sf, int so, CancellationToken cancellationToken)
        {
            var sql = from e in _identityWebContext.Billings.AsNoTracking()

                      where e.AccountId == UserId

                      where string.IsNullOrWhiteSpace(c)
                            || EF.Functions.Like(e.Number, $"%{c}%")

                      select new ViewBillingInfo
                      {
                          GCashSourceResourceId = e.GcashResource!.GcashResourceId,
                          Status = (int)e.Status,
                          GCashCheckoutUrl = e.GcashResource!.CheckoutUrl,
                          DateEnd = e.DateEnd,
                          DateStart = e.DateStart,
                          DateDue = e.DateDue,
                          Amount = e.Amount,
                          BillingId = e.BillingId,
                          StatusText = e.Status.ToString(),
                          Month = e.Month,
                          Number = e.Number,
                          Year = e.Year,
                          KilloWattHourUsed = e.KilloWattHourUsed,
                          Multiplier = e.Multiplier,
                          PresentReading = e.PresentReading,
                          PreviousReading = e.PreviousReading,
                          Reader = e.Reader,
                          ReadingDate = e.ReadingDate,
                          Token = e.ConcurrencyToken,
                          Resource = new WebRazor.ViewModels.Billing.SourceResource
                          {
                              Amount = e.GcashResource == null ? 0 : e.GcashResource.Amount / 100,
                              CheckoutUrl = e.GcashResource == null ? "" : e.GcashResource.CheckoutUrl,
                              //NetAmount = e.GcashResource == null ? 0 : e.GcashResource.net / 100,
                          },
                          Payment = new PaymentResource
                          {
                              GcashPaymentId = e.GcashPayment == null ? "" : e.GcashPayment.GcashPaymentId,
                              Amount = e.GcashPayment == null ? 0 : e.GcashPayment.Amount / 100,
                              Description = e.GcashPayment == null ? "" : e.GcashPayment.Description,
                              Fee = e.GcashPayment == null ? 0 : e.GcashPayment.Fee / 100,
                              NetAmount = e.GcashPayment == null ? 0 : e.GcashPayment!.NetAmount / 100,
                          }

                      };

            var dto = await sql.ToPagedItemsAsync(p, s, cancellationToken);

            //var dto = await _identityWebContext.Billings
            //    .AsNoTracking()
            //    .Where(e => e.AccountId == UserId)
            //    .Select(e => new WebRazor.ViewModels.Billing.ViewBillingInfo
            //    {
            //        GCashSourceResourceId = e.GcashResource!.GcashResourceId,
            //        GCashCheckoutUrl = e.GcashResource!.CheckoutUrl,
            //        DateEnd = e.DateEnd,
            //        DateStart = e.DateStart,
            //        DateDue = e.DateDue,
            //        Amount = e.Amount,
            //        BillingId = e.BillingId,
            //        StatusText = e.Status.ToString(),
            //        Month = e.Month,
            //        Number = e.Number,
            //        Year = e.Year,
            //        KilloWattHourUsed = e.KilloWattHourUsed,
            //        Multiplier = e.Multiplier,
            //        PresentReading = e.PresentReading,
            //        PreviousReading = e.PreviousReading,
            //        Reader = e.Reader,
            //        ReadingDate = e.ReadingDate,
            //        Token = e.ConcurrencyToken,
            //        Resource = new WebRazor.ViewModels.Billing.SourceResource
            //        {
            //            Amount = e.GcashResource == null ? 0 : e.GcashResource.Amount / 100,
            //            CheckoutUrl = e.GcashResource == null ? "" : e.GcashResource.CheckoutUrl,
            //            //NetAmount = e.GcashResource == null ? 0 : e.GcashResource.net / 100,
            //        },
            //        Payment = new PaymentResource
            //        {
            //            GcashPaymentId = e.GcashPayment == null ? "" : e.GcashPayment.GcashPaymentId,
            //            Amount = e.GcashPayment == null ? 0 : e.GcashPayment.Amount / 100,
            //            Description = e.GcashPayment == null ? "" : e.GcashPayment.Description,
            //            Fee = e.GcashPayment == null ? 0 : e.GcashPayment.Fee / 100,
            //            NetAmount = e.GcashPayment == null ? 0 : e.GcashPayment!.NetAmount / 100,
            //        }

            //    })
            //    .ToListAsync();

            return Ok(dto);
        }


        [HttpPost("add-billing")]
        public async Task<IActionResult> AddBilling(AddBillingInfo info, CancellationToken cancellationToken)
        {
            var data = new Data.Identity.Models.Billings.Billing
            {
                BillingId = GuidStr(),
                AccountId = info.AccountId,
                Amount = info.Amount,
                DateDue = info.DateDue,
                DateEnd = info.DateEnd,
                DateStart = info.DateStart,
                Month = info.Month,
                Number = info.Number,
                Year = info.Year,
                KilloWattHourUsed = info.KilloWattHourUsed,
                PresentReading = info.PresentReading,
                PreviousReading = info.PreviousReading,
                Multiplier = info.Multiplier,
                Reader = info.Reader,
                ReadingDate = info.ReadingDate,
            };

            await _identityWebContext.AddAsync(data);

            await _identityWebContext.SaveChangesAsync();

            await _notificationService.AddNotification(data.BillingId, "success", "Billing was created",
                $"New billing was created by administrator: {User.Identity.Name}", DateTime.UtcNow,
                Data.Identity.Models.Notifications.EnumNotificationType.Success, Data.Identity.Models.Notifications.EnumNotificationEntityClass.Billing,
                new[] { info.AccountId }, Array.Empty<string>(), cancellationToken);


            return Ok(data.BillingId);
        }

        [HttpPost("add-billing-resource")]
        public async Task<IActionResult> AddBillingResource(AddBillingSourceInfo info)
        {
            var data = await _identityWebContext.Billings
                .FirstOrDefaultAsync(e => e.BillingId == info.BillingId);

            if (data == null)
                return NotFound("Billing not found.");

            var gcashResource = new GcashResource
            {
                GcashResourceId = info.SourceId,
                BillingId = info.BillingId,

                Amount = info.Amount,
                CheckoutUrl = info.CheckoutUrl,
                Status = info.Status,
            };

            await _identityWebContext.AddAsync(gcashResource);

            await _identityWebContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("{id}/create-resource")]
        public async Task<IActionResult> CreateBillingResource([FromServices] IConfiguration configuration, string id, CancellationToken cancellationToken)
        {
            var item = await _identityWebContext.Billings
                .Include(e => e.GcashResource)
                .Include(e => e.GcashPayment)
                .FirstOrDefaultAsync(e => e.BillingId == id, cancellationToken);

            if (item == null)
                return NotFound("Billing not found.");

            if (item.GcashResource != null)
            {
                _identityWebContext.Remove(item.GcashResource);

                if (item.GcashPayment != null)
                    _identityWebContext.Remove(item.GcashPayment);
            }

            var urlBase = configuration["AppSettings:PayMongo:UrlBase"];
            var publicKey = configuration["AppSettings:PayMongo:PublicKey"];
            var secretKey = configuration["AppSettings:PayMongo:SecretKey"];
            //var urlSuccess = configuration["AppSettings:Payment:UrlSuccess"] + item.BillingId;
            //var urlFailed = configuration["AppSettings:Payment:UrlFailed"] + item.BillingId;

            var urlSuccess = Url.PageLink("/Billings/CheckoutSuccess", values: new { area = "Consumer", id = item.BillingId });
            var urlFailed = Url.PageLink("/Billings/CheckoutFailed", values: new { area = "Consumer", id = item.BillingId });

            var http = new HttpClient();

            var bytes = Encoding.UTF8.GetBytes($"{publicKey}:");
            var base64 = Convert.ToBase64String(bytes);

            var requestContent = new
            {
                data = new
                {
                    attributes = new
                    {
                        type = "gcash",
                        amount = (int)(item.Amount * 100),
                        currency = "PHP",
                        redirect = new
                        {
                            success = urlSuccess,
                            failed = urlFailed,
                        }
                    }
                }
            };

            var sstring = JsonConvert.SerializeObject(requestContent);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{urlBase}sources"),
                Headers =
            {
                { "Accept", "application/json" },
                { "Authorization", $"Basic {base64}" },
            },
                Content = new StringContent(sstring)
                {
                    Headers =
                {
                    ContentType = new MediaTypeHeaderValue("application/json")
                }
                }
            };
            using (var response = await http.SendAsync(request, cancellationToken))
            {

                var body = await response.Content.ReadAsStringAsync(cancellationToken: cancellationToken);

                response.EnsureSuccessStatusCode();

                var sourceResource = await response.Content.ReadFromJsonAsync<ViewModels.GCash.SourceResource>();

                var dataAttributes = sourceResource.Data.Attributes;

                var gcashResource = new GcashResource
                {
                    GcashResourceId = sourceResource.Data.Id,
                    BillingId = item.BillingId,

                    Amount = dataAttributes.Amount,
                    CheckoutUrl = dataAttributes.Redirect.Checkout_Url,
                    Status = dataAttributes.Status,
                };

                await _identityWebContext.AddAsync(gcashResource, cancellationToken);

                await _identityWebContext.SaveChangesAsync(cancellationToken);
            }

            return Ok();
        }

        [HttpPut("{id}/status/{status}")]
        public async Task<IActionResult> UpdateStatus(string id, EnumBillingStatus status, CancellationToken cancellationToken)
        {
            var item = await _identityWebContext.Billings.FirstOrDefaultAsync(e => e.BillingId == id, cancellationToken);

            if (item == null)
                return NotFound("Billing not found.");

            if (item.Status != status)
            {
                item.Status = status;

                if (item.Status == EnumBillingStatus.Unpaid)
                {
                    var user = await _identityWebContext.Users.FirstAsync(e => e.Id == item.AccountId);


                    var url = Url.PageLink("/Billings/View", values: new { area = "Consumer", id = item.BillingId });

                    var fromPhoneNumber = _configuration["Twilio:DefaultPhoneNumber"];
                    var en = Environment.NewLine;
                    var message = $"-{en}{en}Your billing {item.Number} is now ready for payment." +
                        $"{en}Amount: Php{item.Amount}" +
                        $"{en}From: {item.DateStart.Date.ToShortDateString()}." +
                        $"{en}To: {item.DateEnd.Date.ToShortDateString()}." +
                        $"{en}Due: {item.DateDue.Date.ToShortDateString()}." +
                        $"{en}{en}{url}";
                    var toPhoneNumber = user.PhoneNumber;

                    try
                    {
                        await SendSms(fromPhoneNumber, message, toPhoneNumber);
                    }
                    catch (Exception ex)
                    {
                        // swallow exception
                    }
                }

                await _identityWebContext.SaveChangesAsync(cancellationToken);
            }

            return Ok();
        }

        async Task SendSms(string fromPhoneNumber, string message, string toPhoneNumber)
        {
            var accountSid = _configuration["Twilio:AccountSID"];
            var authToken = _configuration["Twilio:AuthToken"];
            TwilioClient.Init(accountSid, authToken);

            try
            {
                var xxx = MessageResource.Create(
                    body: message,
                    from: new Twilio.Types.PhoneNumber(fromPhoneNumber),
                    to: new Twilio.Types.PhoneNumber(toPhoneNumber)
                );

            }
            catch (Exception ex)
            {

                throw;
            }


        }

        async Task<GcashController.SourceResource> GetGcashResource(string resourceId, CancellationToken cancellationToken)
        {
            var ctrl = new GcashController(_identityWebContext, _configuration);

            var resource = await ctrl.GetSourceResourceAsync(resourceId, cancellationToken);

            return resource;

        }
    }
}
