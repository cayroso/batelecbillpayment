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

        [HttpPost]
        public async Task<IActionResult> WebHook([FromBody] WebHookEvent info, CancellationToken cancellationToken)
        {
            if (info!.Data!.Attributes!.Type == "source.chargeable")
            {
                await SourceChargableAsync(info, cancellationToken);
            }
            else
            {

            }
            return Ok();
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

                if (status == "chargeable")
                {
                    var postedPayment = await PostGcashPaymentAsync(gcashResource.GcashResourceId, gcashResource.Amount, $"Payment for Batelec Bill# {billing.Number}", cancellationToken);

                    var paymentInfo = await GetGcashPayment(postedPayment.Data.Id, cancellationToken);

                    var data = new GcashPayment
                    {
                        GcashPaymentId = GuidStr(),
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

        async Task<SourceResource> GetSourceResourceAsync(string resourceId, CancellationToken cancellationToken)
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
                        amount = (int)(amount * 100),
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

        internal class SourceResource
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
        public async Task<IActionResult> GetWebhooks(CancellationToken cancellationToken)
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

                var foo = await response.Content.ReadFromJsonAsync<WebHooks>(options: new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true,
                });

                // update everything 
                var existingWebhooks = await _identityWebContext.GcashWebhooks.ToListAsync(cancellationToken);

                _identityWebContext.RemoveRange(existingWebhooks);

                var items = foo.Data;

                await _identityWebContext.AddRangeAsync(items.Select(e => new GcashWebhook
                {
                    Id = e.Id,
                    Url = e.Attributes.Url,
                    Events = String.Join(',', e.Attributes.Events),
                    LiveMode = e.Attributes.LiveMode,
                    Secret_Key = e.Attributes.Secret_Key,
                    Status = e.Attributes.Status
                }), cancellationToken: cancellationToken);

                await _identityWebContext.SaveChangesAsync(cancellationToken);

                return Ok(foo?.Data);
            }
        }

        [HttpPut("webhooks/{id}/enable")]
        public async Task<IActionResult> EnableWebhook(string id, CancellationToken cancellationToken)
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
                RequestUri = new Uri($"{urlBase}webhooks/{id}/enable"),
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

        [HttpPut("webhooks/{id}/disable")]
        public async Task<IActionResult> DisableWebhook(string id, CancellationToken cancellationToken)
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
                RequestUri = new Uri($"{urlBase}webhooks/{id}/disable"),
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
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync(cancellationToken);
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

        public class EditWebhookInfo
        {
            public string Id { get; set; }
            public string Url { get; set; }
            public IEnumerable<string> Events { get; set; }
        }

        #endregion
    }


}
