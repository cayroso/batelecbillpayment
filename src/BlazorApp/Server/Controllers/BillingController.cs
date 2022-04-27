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
using Data.Identity.Models.Gcash;

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
                .Select(e => new BlazorApp.Shared.Billing.ViewBillingInfo
                {
                    GCashSourceResourceId = e.GcashResource!.GcashResourceId,
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
                .Select(e => new BlazorApp.Shared.Billing.ViewBillingInfo
                {
                    GCashSourceResourceId = e.GcashResource!.GcashResourceId,
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
