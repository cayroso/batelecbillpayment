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

namespace BlazorApp.Server.Controllers
{
    [Route("api/[controller]")]
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
            var isAdmin = User.IsInRole(ApplicationRoles.AdministratorRoleName);

            var dto = await _identityWebContext.Billings
                .AsNoTracking()
                .Where(e => (e.AccountId == UserId || isAdmin) && e.BillingId == billingId)
                .Select(e => new BlazorApp.Shared.Billing.Billing
                {
                    GCashSourceResourceId = e.GcashResource!.GcashResourceId,
                    GCashCheckoutUrl = e.GcashResource!.CheckoutUrl,
                    BillingDateEnd = e.BillingDateEnd,
                    BillingDateStart = e.BillingDateDue,
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
                    ReadingDate = e.ReadingDate,
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

            if (dto == null)
                return BadRequest("Billing not found.");

            return Ok(dto);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Billings()
        {
            var dto = await _identityWebContext.Billings
                .AsNoTracking()
                .Select(e => new BlazorApp.Shared.Billing.Billing
                {
                    GCashSourceResourceId = e.GcashResource!.GcashResourceId,
                    GCashCheckoutUrl = e.GcashResource!.CheckoutUrl,
                    BillingDateEnd = e.BillingDateEnd,
                    BillingDateStart = e.BillingDateDue,
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
                    ReadingDate = e.ReadingDate,
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

        [HttpGet("my-billings")]
        public async Task<IActionResult> MyBillings()
        {
            var dto = await _identityWebContext.Billings
                .AsNoTracking()
                .Where(e => e.AccountId == UserId)
                .Select(e => new BlazorApp.Shared.Billing.Billing
                {
                    GCashSourceResourceId = e.GcashResource!.GcashResourceId,
                    GCashCheckoutUrl = e.GcashResource!.CheckoutUrl,
                    BillingDateEnd = e.BillingDateEnd,
                    BillingDateStart = e.BillingDateDue,
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
                    ReadingDate = e.ReadingDate,
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


        [HttpPost("add-billing")]
        public async Task<IActionResult> AddBilling(AddBillingInfo info)
        {
            var data = new Data.Identity.Models.Billing
            {
                BillingId = GuidStr(),
                AccountId = info.AccountId,
                BillingAmount = info.BillingAmount,
                BillingDateDue = info.BillingDateDue,
                BillingDateEnd = info.BillingDateEnd,
                BillingDateStart = info.BillingDateStart,
                BillingMonth = info.BillingMonth,
                BillingNumber = info.BillingNumber,
                BillingYear = info.BillingYear,
                KilloWattHourUsed = info.KilloWattHourUsed,
                PresentReading = info.PresentReading,
                PreviousReading = info.PreviousReading,
                Multiplier = info.Multiplier,
                Reader = info.Reader,
                ReadingDate = info.ReadingDate,
            };

            await _identityWebContext.AddAsync(data);

            await _identityWebContext.SaveChangesAsync();

            return Ok();
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

    }


}
