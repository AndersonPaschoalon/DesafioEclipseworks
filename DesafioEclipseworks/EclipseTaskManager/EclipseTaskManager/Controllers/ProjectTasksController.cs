using EclipseTaskManager.Context;
using EclipseTaskManager.Models;
using EclipseTaskManager.Repository;
using EclipseTaskManager.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Numerics;

namespace EclipseTaskManager.Controllers;

[Route("v1/api/[controller]")]
[ApiController]
public class ProjectTasksController : ControllerBase
{
    // ActionResult<IEnumerable<ProjectTask>> GetByProject(int projectId)
    // ActionResult<ProjectTask> GetById(int id)
    // ActionResult Post(ProjectTask projectTask)
    // ActionResult Delete(int id)

    private readonly IUserRepository _userRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectTaskRepository _taskRepository;
    private readonly IProjectTaskCommentRepository _commentRepository;
    private readonly IProjectTaskUpdateRepository _updateRepository;
    private readonly ILogger _logger;
    private readonly int MaxTaskByProject = 20;

    public ProjectTasksController(IUserRepository userRepository,
                                  IProjectRepository projectRepository,
                                  IProjectTaskRepository taskRepository,
                                  IProjectTaskCommentRepository commentRepository,
                                  IProjectTaskUpdateRepository updateRepository,
                                  ILogger<ProjectTasksController> logger)
    {
        _userRepository = userRepository;
        _projectRepository = projectRepository;
        _taskRepository = taskRepository;
        _commentRepository = commentRepository;
        _updateRepository = updateRepository;
        _logger = logger;
    }

    /// <summary>
    /// Lists all the tasks of a given project.
    /// </summary>
    /// <param name="projectId"></param>
    /// <returns></returns>
    [HttpGet("all")]
    public ActionResult<IEnumerable<ProjectTask>> GetAllTasks()
    {
        var projectTasks = _taskRepository.GetTasks();
        if (projectTasks is null || !projectTasks.Any())
        {
            return NotFound($"No task found.");
        }
        return Ok(projectTasks);
    }

    /// <summary>
    /// Lists all the tasks of a given project.
    /// </summary>
    /// <param name="projectId"></param>
    /// <returns></returns>
    [HttpGet("byproject")]
    public ActionResult<IEnumerable<ProjectTask>> GetTaskByProject([FromQuery] int id)
    {
        var projectTasks = _taskRepository.GetTasks();
        if (projectTasks is null || !projectTasks.Any())
        {
            return NotFound($"No task found.");
        }
        projectTasks = projectTasks.Where(t => t.ProjectId == id);
        if (projectTasks is null || !projectTasks.Any())
        {
            return NotFound($"No task found for project {id}");
        }
        return Ok(projectTasks);
    }

    /// <summary>
    /// Returns a given task given by its id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}", Name = "GetProjectTask")]
    public ActionResult<ProjectTask> GetTaskById(int id)
    {
        var projectTask = _taskRepository.GetTask(id);
        if (projectTask is null)
        {
            return NotFound($"No task found for id {id}");
        }

        return Ok(projectTask);
    }

    /// <summary>
    /// Create a new task.
    /// </summary>
    /// <param name="projectTask"></param>
    /// <returns></returns>
    [HttpPost]
    public ActionResult PostTask(ProjectTask projectTask)
    {
        // validade/check the project before creating task
        if (projectTask is null)
        {
            return BadRequest("Invalid Project.");
        }

        // project must exist
        var project = _projectRepository.GetProject(projectTask.ProjectId);
        if (project == null)
        {
            return NotFound($"Project ID {projectTask.ProjectId} not found.");
        }

        // user must exist
        var user = _userRepository.GetUser(projectTask.UserId);
        if (user == null)
        {
            return NotFound($"User ID {projectTask.UserId} not found.");
        }

        // the number of tasks must be samaller than MaxTaskByProject
        if (project.ProjectTasks != null && project.ProjectTasks.Count >= MaxTaskByProject)
        {
            return Conflict($"The limit of {MaxTaskByProject} has been rechead. Delete another task to proceed.");
        }

        // clean-up the incomming data
        projectTask = cleanupIncommingTask(projectTask, DateTime.Now);

        // save new project in the database
        try
        {
            _taskRepository.Create(projectTask);
            return new CreatedAtRouteResult("GetProjectTask", new { id = projectTask.ProjectTaskId }, projectTask);
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, $"Database update failed: {ex.Message}");
        }
    }


    /// <summary>
    /// It is not allowed to change the project the task is associated with.
    /// </summary>
    /// <param name="updatedTask"></param>
    /// <returns></returns>
    [HttpPut]
    public ActionResult UpdateTask(ProjectTask updatedTask)
    {
        if (updatedTask == null)
        {
            return BadRequest("Invalid update payload.");
        }
        // check the data
        var user = _userRepository.GetUser(updatedTask.UserId);
        if (user is null)
        {
            return BadRequest($"Invalid user ID {updatedTask.UserId}");
        }
        var project = _projectRepository.GetProject(updatedTask.ProjectId);
        if (project is null)
        {
            return BadRequest($"Invalid project ID {updatedTask.ProjectId}");
        }

        // Retrieve the current entity from the database
        var currentTask = _taskRepository.GetTask(updatedTask.ProjectTaskId);
        if (currentTask == null)
        {
            return NotFound($"Task with ID {updatedTask.ProjectTaskId} not found.");
        }

        // cannot change the task's project
        if (currentTask.ProjectId != updatedTask.ProjectId)
        {
            return Conflict($"You are not allowed to change the Project the Task is associated with. Project ID is {currentTask.ProjectId}");
        }
        // but, you may change the user associated with the task.... no validation for this


        // Detect changes between the current task and the updated task.. 
        // the objects must be cleaned up for a propper comparision. Creation date cannot be changed and inner objects should not be compared
        currentTask = cleanupIncommingTask(currentTask, currentTask.CreationDate);
        updatedTask = cleanupIncommingTask(updatedTask, currentTask.CreationDate); 
        var changes = ObjectHelper.DetectChanges(currentTask, updatedTask);
        if (changes.Count > 0)
        {
            foreach (var change in changes)
            {
                var historyEntry = new ProjectTaskUpdate
                {
                    ModifiedField = change.Key,
                    ModificationDate = DateTime.Now,
                    NewFieldValue = change.Value.NewValue,
                    OldFieldValue = change.Value.OldValue,
                    ProjectId = currentTask.ProjectId,
                    ProjectTaskId = currentTask.ProjectTaskId,
                    UserId = currentTask.UserId,
                };
                _updateRepository.Create(historyEntry);
            }

            // current task is not already concluded and being updated as done
            // we must track how long ago it was set to done!
            if (currentTask.Status != ProjectTask.ProjectTaskStatus.Done && updatedTask.Status == ProjectTask.ProjectTaskStatus.Done)
            {
                updatedTask.ConclusionDate = DateTime.Now;
            }


            // Update the current entity properties
            _taskRepository.Update(updatedTask);
        }

        return new CreatedAtRouteResult("GetProjectTask", new { id = updatedTask.ProjectTaskId }, updatedTask);
    }

    /// <summary>
    /// Delete a task.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    public ActionResult DeleteTask(int id)
    {
        var projectTask = _taskRepository.GetTask(id);
        if (projectTask is null)
        {
            return NotFound($"Project task {projectTask} not found.");
        }

        // delete task
        _taskRepository.Delete(id);

        // delete all tcomments related with task
        var comments = _commentRepository.GetComments().Where(c => c.ProjectTaskId == id);
        foreach (var c in comments)
        {
            _commentRepository.Delete(c.ProjectTaskCommentId);
        }
        // updates history will be kept

        return Ok(projectTask);
    }

    private ProjectTask cleanupIncommingTask(ProjectTask projectTask, DateTime? dt) 
    {
        projectTask.User = null;
        projectTask.Project = null;
        projectTask.Comments = null;
        projectTask.Updates = null;
        projectTask.CreationDate = dt;
        projectTask.Status = ObjectHelper.CastToEnum((int)projectTask.Status, ProjectTask.ProjectTaskStatus.ToDo);
        projectTask.Priority = ObjectHelper.CastToEnum((int)projectTask.Priority, ProjectTask.ProjectTaskPriority.Low);

        return projectTask;
    }

}
