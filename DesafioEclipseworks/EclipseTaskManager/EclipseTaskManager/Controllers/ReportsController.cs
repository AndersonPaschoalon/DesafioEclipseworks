using EclipseTaskManager.Context;
using EclipseTaskManager.Models;
using EclipseTaskManager.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace EclipseTaskManager.Controllers;

[Route("v1/api/[controller]")]
[ApiController]
public class ReportsController : ControllerBase
{

    private readonly IUserRepository _userRepository;
    private readonly IProjectTaskRepository _taskRepository;
    private readonly ILogger _logger;

    public ReportsController(IUserRepository userRepository,
                                  IProjectTaskRepository taskRepository,
                                  ILogger<ReportsController> logger)
    {
        _userRepository = userRepository;
        _taskRepository = taskRepository;
        _logger = logger;
    }

    [HttpGet("report")]
    public ActionResult<Report> GetUsersReport([FromQuery] int id, [FromQuery] int daysPrior = 30)
    {
        // select the user, if the user is not an admin, reject
        var requestingUser = _userRepository.GetUser(id);
        if (requestingUser == null || requestingUser.Role != Models.User.UserRole.Admin)
        {
            return Conflict("User is not allowed to request reports.");
        }

        // query all tasks  filter by conclusion date
        var daysPriorDateTime = DateTime.Now.AddDays(-daysPrior);
        var projectTasks = _taskRepository.GetTasks().Where(t => t.Status == ProjectTask.ProjectTaskStatus.Done && t.ConclusionDate >= daysPriorDateTime);

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

            //report.usersReports = dicRU.Values.ToList();
            report.userReports = dicRU;
        }

        var json = JsonSerializer.Serialize(report);

        return Ok(report);
    }

    [HttpGet("isactive")]
    public ActionResult GetIsActive() 
    {
        return Ok("Service is running.");
    }
}
