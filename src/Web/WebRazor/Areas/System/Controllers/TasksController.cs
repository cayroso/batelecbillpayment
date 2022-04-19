//using App.CQRS;
//using App.CQRS.Tasks.Common.Queries.Query;
//using Cayent.Core.CQRS.Queries;
//using Cayent.Core.Common.Extensions;
//using Data.App.DbContext;
//using Data.App.Models.Users;
//using Data.App.Models.Users.UserTasks;
//using Data.Common;
//using Data.Constants;
//using Data.Enums;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;
//using Web.Areas.Member.Models;
//using Web.Controllers;

//namespace Web.Areas.Manager.Controllers
//{
//    [Authorize(Policy = ApplicationRoles.MemberRoleName)]
//    [ApiController]
//    [Route("api/managers/[controller]")]
//    [Produces("application/json")]
//    public class TasksController : BaseController
//    {
//        readonly IQueryHandlerDispatcher _queryHandlerDispatcher;
//        public TasksController(IQueryHandlerDispatcher queryHandlerDispatcher)
//        {
//            _queryHandlerDispatcher = queryHandlerDispatcher ?? throw new ArgumentNullException(nameof(queryHandlerDispatcher));
//        }

//        [HttpGet]
//        public async Task<IActionResult> Get([FromServices] AppDbContext appDbContext, string c, int p, int s, string sf, int so, EnumTaskStatus ts, EnumTaskType tt)
//        {
//            var sqlTeam = from t in appDbContext.Teams.AsNoTracking()
//                          join tm in appDbContext.TeamMembers on t equals tm.Team
//                          join tm2 in appDbContext.TeamMembers on t equals tm2.Team
//                          where tm.MemberId == UserId
//                          select tm2.MemberId;

//            var dtoTeam = await sqlTeam.Distinct().ToListAsync();

//            var sql = from task in appDbContext.UserTasks.AsNoTracking()

//                      orderby task.DateAssigned descending, task.DateCreated descending

//                      where dtoTeam.Any(e => e == task.UserId)

//                      where string.IsNullOrWhiteSpace(c)
//                             || EF.Functions.Like(task.Title, $"%{c}%")
//                             || EF.Functions.Like(task.Description, $"%{c}%")

//                             || EF.Functions.Like(task.Contact.FirstName, $"%{c}%")
//                             || EF.Functions.Like(task.Contact.MiddleName, $"%{c}%")
//                             || EF.Functions.Like(task.Contact.LastName, $"%{c}%")

//                      where ts == EnumTaskStatus.Unknown || task.Status == ts

//                      where tt == EnumTaskType.Unknown || task.Type == tt

//                      select new
//                      {
//                          TaskId = task.UserTaskId,
//                          task.Title,
//                          //task.Description,
//                          TaskType = task.Type,
//                          TaskTypeText = task.Type.ToString(),
//                          TaskStatus = task.Status,
//                          TaskStatusText = task.Status.ToString(),

//                          Contact = new
//                          {
//                              task.Contact.ContactId,
//                              task.Contact.FirstName,
//                              task.Contact.MiddleName,
//                              task.Contact.LastName
//                          },

//                          DateCreated = task.DateCreated.AsUtc(),
//                          DateAssigned = task.DateAssigned.AsUtc(),
//                          DateCompleted = task.DateCompleted.AsUtc()
//                      };

//            var dto = await sql.ToPagedItemsAsync(p, s);

//            return Ok(dto);
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> Get([FromServices] AppDbContext appDbContext, string id, System.Threading.CancellationToken cancellationToken = default)
//        {
//            var query = new GetTaskByIdQuery("", TenantId, UserId, id);

//            var dto = await _queryHandlerDispatcher.HandleAsync<GetTaskByIdQuery, GetTaskByIdQuery.Task>(query, cancellationToken);

//            return Ok(dto);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Add([FromServices] AppDbContext appDbContext)
//        {
//            var infoFile = Request.Form.Files.FirstOrDefault(e => e.Name == "info");

//            if (infoFile != null && infoFile.Length > 0)
//            {
//                var streamReader = new StreamReader(infoFile.OpenReadStream());

//                var infoJson = streamReader.ReadToEnd();

//                var info = JsonConvert.DeserializeObject<AddTaskInfo>(infoJson);

//                var taskItemNumber = 1;

//                var data = new UserTask
//                {
//                    UserTaskId = GuidStr(),
//                    Type = info.TaskType,
//                    Status = Data.Enums.EnumTaskStatus.Todo,
//                    ContactId = info.ContactId,
//                    Title = info.Title,
//                    Description = info.Description,
//                    UserId = UserId,
//                    RoleId = ApplicationRoles.Member.Id,

//                    UserTaskItems = info.TaskItems.Select(e => new UserTaskItem
//                    {
//                        UserTaskItemId = GuidStr(),
//                        Number = taskItemNumber++,
//                        Title = e.Title,
//                        IsDone = false,
//                    }).ToList(),

//                    DateCreated = DateTime.UtcNow,
//                    DateAssigned = DateTime.UtcNow,
//                    DateCompleted = info.DateToComplete,
//                    DateActualCompleted = DateTime.MaxValue,
//                };

//                await appDbContext.AddAsync(data);

//                await appDbContext.SaveChangesAsync();

//                return Ok(data.UserTaskId);
//            }

//            return BadRequest();
//        }

//        [HttpDelete("{id}/{token}")]
//        public async Task<IActionResult> Delete([FromServices] AppDbContext appDbContext, string id, string token)
//        {

//            if (ModelState.IsValid)
//            {
//                var data = await appDbContext.UserTasks.Include(e => e.UserTaskItems).FirstOrDefaultAsync(e => e.UserTaskId == id);

//                data.ThrowIfNullOrAlreadyUpdated(token, GuidStr());

//                appDbContext.Remove(data);

//                await appDbContext.SaveChangesAsync();

//                return Ok();
//            }

//            return BadRequest();
//        }

//        [HttpPut("update-task-item/{taskId}/{taskItemId}/{isDone}")]
//        public async Task<IActionResult> UpdateTaskitem([FromServices] AppDbContext appDbContext, string taskId, string taskItemId, bool isDone)
//        {
//            var task = await appDbContext.UserTasks.Include(e => e.UserTaskItems).FirstOrDefaultAsync(e => e.UserTaskId == taskId);
//            task.ThrowIfNull();

//            var taskItem = task.UserTaskItems.FirstOrDefault(e => e.UserTaskItemId == taskItemId);
//            taskItem.ThrowIfNull();

//            taskItem.IsDone = isDone;
//            taskItem.DateCompleted = isDone ? DateTime.UtcNow : DateTime.MaxValue;

//            if (task.UserTaskItems.All(e => !e.IsDone))
//            {
//                task.Status = Data.Enums.EnumTaskStatus.Todo;
//                task.DateActualCompleted = DateTime.MaxValue;
//            }

//            if (task.UserTaskItems.Any(e => e.IsDone))
//            {
//                task.Status = Data.Enums.EnumTaskStatus.InProgress;
//                task.DateActualCompleted = DateTime.MaxValue;
//            }

//            if (task.UserTaskItems.All(e => e.IsDone))
//            {
//                task.Status = Data.Enums.EnumTaskStatus.Done;
//                task.DateActualCompleted = DateTime.UtcNow;
//            }

//            await appDbContext.SaveChangesAsync();

//            return Ok();
//        }
//    }
//}
