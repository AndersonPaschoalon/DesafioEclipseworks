using EclipseTaskManager.Context;
using EclipseTaskManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EclipseTaskManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly EclipseTaskManagerContext _context;

        public ReportsController(EclipseTaskManagerContext context)
        {
            _context = context;
        }

        [HttpGet("userId:int")]
        public ActionResult<Report> GetUsersReport(int userId, [FromQuery] int daysPrior = 30)
        {
            // select the user, if the user is not an admin, reject
            var requestingUser = _context.Users.AsNoTracking().FirstOrDefault(u => u.UserId == userId);
            if (requestingUser == null || requestingUser.Role != Models.User.UserRole.Admin)
            {
                return Conflict("User is not allowed to request reports.");
            }

            // query all tasks  filter by conclusion date
            var daysPriorDateTime = DateTime.Now.AddDays(-daysPrior);
            var projectTasks = _context.ProjectTasks
                        .AsNoTracking()
                        .Where(t => t.Status == ProjectTask.ProjectTaskStatus.Done && t.ConclusionDate >= daysPriorDateTime);

            Report report = new Report();
            if (projectTasks is not null) 
            {
                

                Dictionary<int, ReportUser> dicRU = new Dictionary<int, ReportUser>();
                foreach (var task in projectTasks) 
                {
                    if (dicRU.ContainsKey(task.UserId))
                    {
                        dicRU[task.UserId].tasksInDone.Add(task.ProjectTaskId);
                    }
                    else
                    {
                        ReportUser ru = new ReportUser();
                        ru.userId = task.UserId;
                        ru.tasksInDone = new List<int> { task.ProjectTaskId };
                        dicRU.Add(task.UserId, ru);
                    }
                }
                foreach (var entry in dicRU) 
                {
                    report.usersReports.Add(entry.Value);
                }
            }
            return Ok(report);
        }
    }
}
