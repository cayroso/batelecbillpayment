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
using BlazorApp.Shared.Billing;
using Data.Identity.Models;

namespace Blazor.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BillingController : BaseController
    {
        IdentityWebContext _identityWebContext;
        public BillingController(IdentityWebContext identityWebContext)
        {
            _identityWebContext = identityWebContext;
        }

        [HttpGet("{billingId}")]
        public async Task<IActionResult> Get(string billingId)
        {
            var dto = await _identityWebContext.Billings
                .AsNoTracking()
                .Where(e => e.AccountId == UserId && e.BillingId == billingId)
                .Select(e => new BlazorApp.Shared.Billing.Billing
                {
                    GCashSourceResourceId = e.GcashResource!.GcashResourceId,
                    GCashCheckoutUrl = e.GcashResource!.CheckoutUrl,
                    BillDateEnd = e.BillDateEnd,
                    BillDateStart = e.BillingDateDue,
                    BillingDateDue = e.BillingDateDue,
                    BillingAmount = e.BillingAmount,
                    BillingId = e.BillingId,
                    BillingMonth = e.BillingMonth,
                    BillingNumber = e.BillingNumber,
                    BillingYear = e.BillingYear,
                    KilloWattHourUsed = e.KilloWattHourUsed,
                    Multiplier = e.Multiplier,
                    PresentReading = e.PresentReading,
                    PreviousReading = e.PreviousReading,
                    Reader = e.Reader,
                    ReadingTime = e.ReadingTime,
                    Token = e.ConcurrencyToken,
                    Resource = new BlazorApp.Shared.Billing.SourceResource
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
                .FirstOrDefaultAsync();

            return Ok(dto);
        }

        [HttpGet]
        public async Task<IActionResult> MyBillings()
        {
            var dto = await _identityWebContext.Billings
                .AsNoTracking()
                .Where(e => e.AccountId == UserId)
                .Select(e => new BlazorApp.Shared.Billing.Billing
                {
                    GCashSourceResourceId = e.GcashResource!.GcashResourceId,
                    GCashCheckoutUrl = e.GcashResource!.CheckoutUrl,
                    BillDateEnd = e.BillDateEnd,
                    BillDateStart = e.BillingDateDue,
                    BillingDateDue = e.BillingDateDue,
                    BillingAmount = e.BillingAmount,
                    BillingId = e.BillingId,
                    BillingMonth = e.BillingMonth,
                    BillingNumber = e.BillingNumber,
                    BillingYear = e.BillingYear,
                    KilloWattHourUsed = e.KilloWattHourUsed,
                    Multiplier = e.Multiplier,
                    PresentReading = e.PresentReading,
                    PreviousReading = e.PreviousReading,
                    Reader = e.Reader,
                    ReadingTime = e.ReadingTime,
                    Token = e.ConcurrencyToken,
                    Resource = new BlazorApp.Shared.Billing.SourceResource
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
                .ToListAsync();

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> AddBillingResource(AddBillingSourceInfo info)
        {
            var data = await _identityWebContext.Billings
                .FirstOrDefaultAsync(e => e.BillingId == info.BillingId);

            if (data == null)
                return NotFound();

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

    }


}
