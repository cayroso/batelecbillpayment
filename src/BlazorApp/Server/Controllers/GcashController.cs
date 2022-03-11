using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Blazor.Shared;
using Data.Identity.Models.Users;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Blazor.Shared.Security;
using Data.Identity.DbContext;
using Data.Constants;
using Microsoft.EntityFrameworkCore;
using BlazorApp.Shared.GCash;
using Data.Identity.Models;
using System.Net.Http;
using System.Net.Http.Headers;

namespace BlazorApp.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GcashController : BaseController
    {
        IdentityWebContext _identityWebContext;
        public GcashController(IdentityWebContext identityWebContext)
        {
            _identityWebContext = identityWebContext;
        }

        [HttpPost]
        public async Task<IActionResult> WebHook([FromBody] WebHookEvent info)
        {
            if (info!.Data!.Attributes!.Type == "source.chargeable")
            {
                await SourceChargable(info);
            }
            else
            {

            }
            return Ok();
        }

        async Task SourceChargable(WebHookEvent info)
        {
            var foo = info.Data.Attributes.Data.Attributes;

            var sourceId = info.Data.Attributes.Data.Id;

            var gcashResource = await _identityWebContext.GcashResources.FirstOrDefaultAsync(e => e.GcashResourceId == sourceId);

            if (gcashResource != null)
            {
                var billing = await _identityWebContext.Billings.FirstAsync(e => e.BillingId == gcashResource.BillingId);

                var sourceResource = await GetSourceResource(gcashResource.GcashResourceId);
                var status = sourceResource.Data.Attributes.Status;

                if (status == "chargeable")
                {
                    var postedPayment = await PostGcashPayment(gcashResource.GcashResourceId, gcashResource.Amount, $"Payment for Batelec Bill# {billing.BillingNumber}");

                    var paymentInfo = await GetGcashPayment(postedPayment.Data.Id);

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

                    await _identityWebContext.AddAsync(data);

                    gcashResource.Status = data.Status;
                }
                else if (status == "paid")
                {

                }
                else
                {

                }

                

                await _identityWebContext.SaveChangesAsync();                

            }
        }

        async Task<SourceResource> GetSourceResource(string resourceId)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://api.paymongo.com/v1/sources/{resourceId}"),
                Headers =
                {
                    { "Accept", "application/json" },
                    { "Authorization", "Basic c2tfdGVzdF9Dd2JBeFpORGFjSlV4RTRTSGpUeHdrdUc6" },
                },
            };

            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();

                var data = await response.Content.ReadFromJsonAsync<SourceResource>();

                return data; ;
                //var body = await response.Content.ReadAsStringAsync();
                //Console.WriteLine(body);
            }
        }

        async Task<PaymentSource> PostGcashPayment(string resourceId, double amount, string description)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://api.paymongo.com/v1/payments"),
                Headers =
                {
                    { "Accept", "application/json" },
                    { "Authorization", "Basic c2tfdGVzdF9Dd2JBeFpORGFjSlV4RTRTSGpUeHdrdUc6" },
                },
                Content = new StringContent("{\"data\":{\"attributes\":{\"amount\":" + amount.ToString() + ",\"source\":{\"id\":\"" + resourceId + "\",\"type\":\"source\"},\"description\":\"" + description + "\",\"currency\":\"PHP\"}}}")
                {
                    Headers =
                    {
                        ContentType = new MediaTypeHeaderValue("application/json")
                    }
                }
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();

                var foo = await response.Content.ReadFromJsonAsync<PaymentSource>();

                return foo;
            }
        }

        async Task<PaymentSource> GetGcashPayment(string resourceId)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://api.paymongo.com/v1/payments/{resourceId}"),
                Headers =
                {
                    { "Accept", "application/json" },
                    { "Authorization", "Basic c2tfdGVzdF9Dd2JBeFpORGFjSlV4RTRTSGpUeHdrdUc6" },
                },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();

                var foo = await response.Content.ReadFromJsonAsync<PaymentSource>();

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
    }


}
