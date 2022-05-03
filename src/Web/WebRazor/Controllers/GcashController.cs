using Microsoft.AspNetCore.Mvc;
using Data.Identity.DbContext;
using Microsoft.EntityFrameworkCore;
using WebRazor.ViewModels.GCash;
using Data.Identity.Models;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Data.Identity.Models.Gcash;
using System.Text.Json;
using Cayent.Core.Common.Extensions;

namespace WebRazor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GcashController : BaseController
    {
        IdentityWebContext _identityWebContext;
        private readonly IConfiguration _configuration;

        public GcashController(IdentityWebContext identityWebContext, IConfiguration configuration)
        {
            _identityWebContext = identityWebContext;
            _configuration = configuration;
        }

        [HttpPost("webhook-process")]
        public async Task<IActionResult> WebHook([FromBody] WebHookEvent info, CancellationToken cancellationToken)
        {
            var eventType = info!.Data!.Attributes!.Type;

            if (eventType == "source.chargeable")
            {
                await SourceChargableAsync(info, cancellationToken);

                return Ok();
            }
            else if (eventType == "payment.paid")
            {                
                var sourceId = info.Data.Attributes.Data.Id;
                var gcashPayment = await _identityWebContext.GcashPayments.FirstOrDefaultAsync(e => e.GcashPaymentId == sourceId, cancellationToken);

                if (gcashPayment != null)
                {
                    var billing = await _identityWebContext.Billings.FirstAsync(e => e.BillingId == gcashPayment.BillingId, cancellationToken);
                    
                    billing.Status = Data.Identity.Models.Billings.EnumBillingStatus.Paid;

                    await _identityWebContext.SaveChangesAsync(cancellationToken);

                    return Ok();
                }
            }
            else
            {
                return Ok();
            }

            return BadRequest();
        }

        async Task SourceChargableAsync(WebHookEvent info, CancellationToken cancellationToken)
        {
            var foo = info.Data.Attributes.Data.Attributes;

            var sourceId = info.Data.Attributes.Data.Id;

            var gcashResource = await _identityWebContext.GcashResources.FirstOrDefaultAsync(e => e.GcashResourceId == sourceId, cancellationToken);

            if (gcashResource != null)
            {
                var billing = await _identityWebContext.Billings.FirstAsync(e => e.BillingId == gcashResource.BillingId, cancellationToken);

                var sourceResource = await GetSourceResourceAsync(gcashResource.GcashResourceId, cancellationToken);
                var status = sourceResource.Data.Attributes.Status;

                //  check if we already processed this
                if (gcashResource.Status == "paid")
                    return;

                if (status == "chargeable")
                {
                    var postedPayment = await PostGcashPaymentAsync(gcashResource.GcashResourceId, gcashResource.Amount, $"Payment for Batelec Bill# {billing.Number}", cancellationToken);

                    var paymentInfo = await GetGcashPayment(postedPayment.Data.Id, cancellationToken);

                    var data = new GcashPayment
                    {
                        GcashPaymentId = paymentInfo.Data.Id,
                        BillingId = gcashResource.BillingId,
                        //GcashResourceId = gcashResource.GcashResourceId,

                        AccessUrl = paymentInfo!.Data!.Attributes!.Access_Url,
                        Amount = paymentInfo.Data.Attributes.Amount.GetValueOrDefault(),
                        BalanceTransactionId = paymentInfo.Data.Attributes.Balance_Transaction_Id,
                        Currency = paymentInfo.Data.Attributes.Currency,
                        Description = paymentInfo.Data.Attributes.Description,
                        ExternalReferenceNumber = paymentInfo.Data.Attributes.External_Reference_Number,
                        Fee = paymentInfo.Data.Attributes.Fee.GetValueOrDefault(),
                        NetAmount = paymentInfo.Data.Attributes.Net_Amount.GetValueOrDefault(),
                        StatementDescriptor = paymentInfo.Data.Attributes.Statement_Descriptor,
                        Status = paymentInfo.Data.Attributes.Status,
                    };

                    //billing.Status = Data.Identity.Models.Billings.EnumBillingStatus.Paid;

                    await _identityWebContext.AddAsync(data, cancellationToken);

                    gcashResource.Status = data.Status;
                }
                else if (status == "paid")
                {

                }
                else
                {

                }



                await _identityWebContext.SaveChangesAsync(cancellationToken);

            }
        }

        [HttpGet("resource/{resourceId}")]
        public async Task<SourceResource> GetSourceResourceAsync(string resourceId, CancellationToken cancellationToken)
        {
            var urlBase = _configuration["AppSettings:PayMongo:UrlBase"];
            var publicKey = _configuration["AppSettings:PayMongo:PublicKey"];
            var secretKey = _configuration["AppSettings:PayMongo:SecretKey"];
            var bytes = Encoding.UTF8.GetBytes($"{publicKey}:");
            var base64 = Convert.ToBase64String(bytes);

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{urlBase}sources/{resourceId}"),
                Headers =
                {
                    { "Accept", "application/json" },
                    { "Authorization", $"Basic {base64}" },
                },
            };

            using (var response = await client.SendAsync(request, cancellationToken))
            {
                response.EnsureSuccessStatusCode();

                var data = await response.Content.ReadFromJsonAsync<SourceResource>(cancellationToken: cancellationToken);

                return data;
                //var body = await response.Content.ReadAsStringAsync();
                //Console.WriteLine(body);
            }
        }

        async Task<PaymentSource> PostGcashPaymentAsync(string resourceId, double amount, string description, CancellationToken cancellationToken)
        {
            var urlBase = _configuration["AppSettings:PayMongo:UrlBase"];
            var publicKey = _configuration["AppSettings:PayMongo:PublicKey"];
            var secretKey = _configuration["AppSettings:PayMongo:SecretKey"];
            var bytes = Encoding.UTF8.GetBytes($"{secretKey}:");
            var base64 = Convert.ToBase64String(bytes);

            var foo = new
            {
                data = new
                {
                    attributes = new
                    {
                        amount = (int)amount,
                        description = description,
                        currency = "PHP",
                        source = new
                        {
                            id = resourceId,
                            type = "source"
                        }
                    }
                }
            };
            var bar = JsonConvert.SerializeObject(foo);

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{urlBase}payments"),
                Headers =
                {
                    { "Accept", "application/json" },
                    { "Authorization", $"Basic {base64}" },
                },
                Content = new StringContent(bar)
                //Content = new StringContent("{\"data\":{\"attributes\":{\"amount\":" + amount.ToString() + ",\"source\":{\"id\":\"" + resourceId + "\",\"type\":\"source\"},\"description\":\"" + description + "\",\"currency\":\"PHP\"}}}")
                {
                    Headers =
                    {
                        ContentType = new MediaTypeHeaderValue("application/json")
                    }
                }
            };
            using (var response = await client.SendAsync(request, cancellationToken))
            {
                if (!response.IsSuccessStatusCode)
                {
                    var foor = await response.Content.ReadAsStringAsync(cancellationToken);
                }

                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<PaymentSource>(cancellationToken: cancellationToken);

                return result;
            }
        }

        async Task<PaymentSource> GetGcashPayment(string resourceId, CancellationToken cancellationToken)
        {
            var urlBase = _configuration["AppSettings:PayMongo:UrlBase"];
            var publicKey = _configuration["AppSettings:PayMongo:PublicKey"];
            var secretKey = _configuration["AppSettings:PayMongo:SecretKey"];
            var bytes = Encoding.UTF8.GetBytes($"{secretKey}:");
            var base64 = Convert.ToBase64String(bytes);

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{urlBase}payments/{resourceId}"),
                Headers =
                {
                    { "Accept", "application/json" },
                    { "Authorization", $"Basic {base64}" },
                },
            };
            using (var response = await client.SendAsync(request, cancellationToken))
            {
                response.EnsureSuccessStatusCode();

                var foo = await response.Content.ReadFromJsonAsync<PaymentSource>(cancellationToken: cancellationToken);

                return foo;

            }
        }

        public class SourceResource
        {
            public SourceResourceInfo Data { get; set; }

            public class SourceResourceInfo
            {
                public string Id { get; set; }
                public AttributesInfo Attributes { get; set; }

                public class AttributesInfo
                {
                    public double Amount { get; set; }
                    public string Description { get; set; }
                    public string Status { get; set; }
                    public RedirectInfo Redirect { get; set; }
                }

                public class RedirectInfo
                {
                    public string Checkout_Url { get; set; }
                    public string Success { get; set; }
                    public string Failed { get; set; }
                }

            }
        }

        internal class PaymentSource
        {
            public PaymentSourceData Data { get; set; }

            public class PaymentSourceData
            {
                public string Id { get; set; }
                public string Type { get; set; }
                public PaymentSourceDataAttribute Attributes { get; set; }
            }

            public class PaymentSourceDataAttribute
            {
                public string? Access_Url { get; set; }
                public double? Amount { get; set; }
                public string? Balance_Transaction_Id { get; set; }
                public string? Currency { get; set; }
                public string? Description { get; set; }
                public string? External_Reference_Number { get; set; }

                public double? Fee { get; set; }
                public bool? LiveMode { get; set; }
                public double? Net_Amount { get; set; }
                //public string Payout { get; set; }
                public string? Statement_Descriptor { get; set; }
                public string? Status { get; set; }
                public double? Tax_Amount { get; set; }
                public ulong? Available_At { get; set; }

                public ulong? Created_At { get; set; }
                public ulong? Paid_At { get; set; }
                public ulong? Updated_At { get; set; }

                public PaymentSourceDataAttributeSource Source { get; set; }
            }

            public class PaymentSourceDataAttributeSource
            {
                public string Id { get; set; }
                public string Type { get; set; }
            }
        }


        #region Webhook

        [HttpGet("webhooks")]
        public async Task<IActionResult> GetWebhooks(string c, int p, int s, string sf, int so, CancellationToken cancellationToken)
        {
            var urlBase = _configuration["AppSettings:PayMongo:UrlBase"];
            var publicKey = _configuration["AppSettings:PayMongo:PublicKey"];
            var secretKey = _configuration["AppSettings:PayMongo:SecretKey"];
            var bytes = Encoding.UTF8.GetBytes($"{secretKey}:");
            var base64 = Convert.ToBase64String(bytes);

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{urlBase}webhooks"),
                Headers =
                {
                    { "Accept", "application/json" },
                    { "Authorization", $"Basic {base64}" },
                },
            };
            using (var response = await client.SendAsync(request, cancellationToken))
            {
                response.EnsureSuccessStatusCode();

                var foo = await response.Content.ReadFromJsonAsync<WebHooks>();

                // update everything 
                var existingWebhooks = await _identityWebContext.GcashWebhooks.ToListAsync(cancellationToken);

                _identityWebContext.RemoveRange(existingWebhooks);

                var items = foo.Data;

                await _identityWebContext.AddRangeAsync(items.Select(e => new GcashWebhook
                {
                    Id = e.Id,
                    Url = e.Attributes.Url,
                    Events = String.Join(",", e.Attributes.Events),
                    LiveMode = e.Attributes.LiveMode,
                    Secret_Key = e.Attributes.Secret_Key,
                    Status = e.Attributes.Status
                }), cancellationToken: cancellationToken);

                await _identityWebContext.SaveChangesAsync(cancellationToken);

                var sql = from wh in _identityWebContext.GcashWebhooks.AsNoTracking()
                          select wh;

                var dto = await sql.ToPagedItemsAsync(p, s, cancellationToken);

                return Ok(dto);
            }
        }

        [HttpGet("webhooks/{id}")]
        public async Task<IActionResult> GetWebhooks(string id, CancellationToken cancellationToken)
        {
            var urlBase = _configuration["AppSettings:PayMongo:UrlBase"];
            var publicKey = _configuration["AppSettings:PayMongo:PublicKey"];
            var secretKey = _configuration["AppSettings:PayMongo:SecretKey"];
            var bytes = Encoding.UTF8.GetBytes($"{secretKey}:");
            var base64 = Convert.ToBase64String(bytes);

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{urlBase}webhooks/{id}"),
                Headers =
                {
                    { "Accept", "application/json" },
                    { "Authorization", $"Basic {base64}" },
                },
            };
            using (var response = await client.SendAsync(request, cancellationToken))
            {
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<WebHookWrapper>();

                var foo = result.Data;

                var gwh = new GcashWebhook
                {
                    Id = foo.Id,
                    Url = foo.Attributes.Url,
                    Events = String.Join(",", foo.Attributes.Events),
                    LiveMode = foo.Attributes.LiveMode,
                    Secret_Key = foo.Attributes.Secret_Key,
                    Status = foo.Attributes.Status
                };

                return Ok(gwh);
                // update everything 
                //var existingWebhooks = await _identityWebContext.GcashWebhooks.ToListAsync(cancellationToken);

                //_identityWebContext.RemoveRange(existingWebhooks);

                //var items = foo.Data;

                //await _identityWebContext.AddRangeAsync(items.Select(e => new GcashWebhook
                //{
                //    Id = e.Id,
                //    Url = e.Attributes.Url,
                //    Events = String.Join(", ", e.Attributes.Events),
                //    LiveMode = e.Attributes.LiveMode,
                //    Secret_Key = e.Attributes.Secret_Key,
                //    Status = e.Attributes.Status
                //}), cancellationToken: cancellationToken);

                //await _identityWebContext.SaveChangesAsync(cancellationToken);

                //var sql = from wh in _identityWebContext.GcashWebhooks.AsNoTracking()
                //          select wh;

                //var dto = await sql.ToPagedItemsAsync(p, s, cancellationToken);

                //return Ok(dto);
            }

            return Ok();
        }

        [HttpPut("webhooks/{id}/enable-disable/{flag}")]
        public async Task<IActionResult> EnableDisableWebhook(string id, bool flag, CancellationToken cancellationToken)
        {
            var urlBase = _configuration["AppSettings:PayMongo:UrlBase"];
            var publicKey = _configuration["AppSettings:PayMongo:PublicKey"];
            var secretKey = _configuration["AppSettings:PayMongo:SecretKey"];
            var bytes = Encoding.UTF8.GetBytes($"{secretKey}:");
            var base64 = Convert.ToBase64String(bytes);

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{urlBase}webhooks/{id}/{(flag ? "enable" : "disable")}"),
                Headers =
                {
                    { "Accept", "application/json" },
                    { "Authorization", $"Basic {base64}" },
                },
            };
            using (var response = await client.SendAsync(request, cancellationToken))
            {
                response.EnsureSuccessStatusCode();

                var foo = await response.Content.ReadAsStringAsync(cancellationToken);

                return Ok(foo);
            }
        }

        [HttpPut("webhooks")]
        public async Task<IActionResult> UpdateWebhook([FromBody] EditWebhookInfo info, CancellationToken cancellationToken)
        {
            var data = await _identityWebContext.GcashWebhooks.FirstOrDefaultAsync(e => e.Id == info.Id, cancellationToken);

            if (data == null)
                return NotFound("Webhook not found.");

            var urlBase = _configuration["AppSettings:PayMongo:UrlBase"];
            var publicKey = _configuration["AppSettings:PayMongo:PublicKey"];
            var secretKey = _configuration["AppSettings:PayMongo:SecretKey"];
            var bytes = Encoding.UTF8.GetBytes($"{secretKey}:");
            var base64 = Convert.ToBase64String(bytes);

            var foo = new
            {
                data = new
                {
                    attributes = new
                    {
                        url = info.Url,
                        events = info.Events
                    }
                }
            };
            var bar = JsonConvert.SerializeObject(foo);

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"{urlBase}webhooks/{info.Id}"),
                Headers =
                {
                    { "Accept", "application/json" },
                    { "Authorization", $"Basic {base64}" },
                },
                Content = new StringContent(bar)
                {
                    Headers =
                    {
                        ContentType = new MediaTypeHeaderValue("application/json")
                    }
                }
            };
            using (var response = await client.SendAsync(request, cancellationToken))
            {
                //response.EnsureSuccessStatusCode();

                if (!response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync(cancellationToken);

                    return BadRequest(result);
                }
            }

            return Ok();
        }

        public class WebHooks
        {
            public IEnumerable<WebHook> Data { get; set; }

            public class WebHook
            {
                public string Id { get; set; }
                public string Type { get; set; }

                public WebHookAttribute Attributes { get; set; }

                public class WebHookAttribute
                {
                    public bool LiveMode { get; set; }
                    public string Secret_Key { get; set; }
                    public string Status { get; set; }
                    public string Url { get; set; }
                    public IEnumerable<string> Events { get; set; }
                }
            }


        }

        public class WebHookWrapper
        {
            public WebHooks.WebHook Data { get; set; }
        }

        public class EditWebhookInfo
        {
            public string Id { get; set; }
            public string Url { get; set; }
            public IEnumerable<string> Events { get; set; }
        }

        #endregion
    }


}
