//using App.CQRS;
//using Cayent.Core.CQRS.Queries;
//using Data.App.DbContext;
//using Data.Constants;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Web.Controllers;

//namespace Web.Areas.Manager.Controllers
//{
//    [Authorize(Policy = ApplicationRoles.ManagerRoleName)]
//    [ApiController]
//    [Route("api/managers/[controller]")]
//    [Produces("application/json")]
//    public class DefaultController : BaseController
//    {
//        readonly IQueryHandlerDispatcher _queryHandlerDispatcher;
//        public DefaultController(IQueryHandlerDispatcher queryHandlerDispatcher)
//        {
//            _queryHandlerDispatcher = queryHandlerDispatcher ?? throw new ArgumentNullException(nameof(queryHandlerDispatcher));
//        }

//        [HttpGet("dashboard")]
//        public async Task<IActionResult> GetDashboard([FromServices] AppDbContext dbContext, int year, int month)
//        {
//            var now = DateTime.UtcNow;
//            var startDate = new DateTime(now.Year, now.Month, 1);
//            var endDate = startDate.AddMonths(1).AddDays(-1);

//            //  number of teams
//            var teams = await dbContext.TeamMembers.AsNoTracking().Where(m => m.MemberId == UserId).CountAsync();

//            //  number of members
//            var users = await dbContext.Teams
//                            .AsNoTracking()
//                            .Where(e => e.Members.Any(m => m.MemberId == UserId))
//                            .SelectMany(e => e.Members).Select(e => e.MemberId)
//                            .Distinct()
//                            .CountAsync();
            
//            //  number of contacts
//            var contacts = await dbContext.Contacts.CountAsync();

//            //  number of documens
//            var documents = await dbContext.Documents.CountAsync();

//            //  number of attachments
//            var attachments = await dbContext.ContactAttachments.CountAsync();

//            //  number of tasks
//            var tasks = await dbContext.UserTasks.CountAsync();

//            var dto = new
//            {
//                teams,
//                users,
//                contacts,
//                documents,
//                attachments,
//                tasks
//            };

//            return Ok(dto);
//        }
//    }
//}
