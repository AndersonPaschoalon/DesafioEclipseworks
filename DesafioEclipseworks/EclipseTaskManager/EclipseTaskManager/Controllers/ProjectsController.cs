using EclipseTaskManager.Context;
using EclipseTaskManager.Models;
using EclipseTaskManager.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace EclipseTaskManager.Controllers;

[Route("v1/api/[controller]")]
[ApiController]
public class ProjectsController : ControllerBase
{
    // ActionResult<IEnumerable<Project>> GetByUser(int userId)
    // ActionResult<Project> GetById(int id)
    // ActionResult Post(Project project)
    // ActionResult Put(Project project)
    // ActionResult Delete(int id)

    private readonly IProjectRepository _projectRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger _logger;

    public ProjectsController(IProjectRepository projectRepository, IUserRepository userRepository, ILogger<ProjectsController> logger)
    {
        //_context = context;
        _projectRepository = projectRepository;
        _userRepository = userRepository;
        _logger = logger;
    }

    [HttpGet("all")]
    public ActionResult<IEnumerable<Project>> GetAll()
    {
        // retrive the user projects
        var projects = _projectRepository.GetProjects();
        if (projects is null || !projects.Any())
        {
            return NotFound($"No project found.");
        }

        return Ok(projects);

    }

    /// <summary>
    /// Returns all projects of a given user specified by its user ID.
    /// </summary>
    /// <param name="userId">User Id</param>
    /// <returns>List of projects its user owns</returns>
    [HttpGet("byuser")]
    public ActionResult<IEnumerable<Project>> GetByUser([FromQuery] int userId)
    {
        _logger.LogInformation($"===================GET:ProjectsController:GetByUser:userId={userId}===================");

        // check if the user exists
        var user = _userRepository.GetUser(userId);
        if (user == null)
        {
            return NotFound($"UserId {userId} not found.");
        }

        // retrive the user projects
        var projects = _projectRepository.GetProjects().Where(p => p.UserId == userId);
        if (projects is null || !projects.Any())
        {
            return NotFound($"No project found for user {userId}");
        }

        return Ok(projects);

    }

    /// <summary>
    /// Retrieve a given projectby its user ID.
    /// </summary>
    /// <param name="id">Project ID.</param>
    /// <returns>Project</returns>
    [HttpGet("{id:int}", Name = "GetProject")]
    public ActionResult<Project> GetById(int id)
    {
        var project = _projectRepository.GetProject(id);
        if (project is null)
        {
            return NotFound($"No Project found for ID {id}");
        }
        return Ok(project);
    }

    /// <summary>
    /// Create a newe project
    /// </summary>
    /// <param name="project"></param>
    /// <returns></returns>
    [HttpPost]
    public ActionResult Post(Project project)
    {
        // check project format
        if (project is null)
        {
            return BadRequest("Invalid Project.");
        }

        // check user id
        int userId = project.UserId;
        var user = _userRepository.GetUser(userId);
        if (user is null)
        {
            return NotFound($"UserId {userId} not found.");
        }

        // save new project in the database
        try
        {
            _projectRepository.Create(project);
            return new CreatedAtRouteResult("GetProject", new { id = project.ProjectId }, project);
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, $"Database update failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Update project.
    /// </summary>
    /// <param name="project"></param>
    /// <returns></returns>
    [HttpPut]
    public ActionResult Put(Project project)
    {
        // check project format
        if (project is null)
        {
            return BadRequest("Invalid Project.");
        }

        // validation: project and user must exist.
        var currProj = _projectRepository.GetProject(project.ProjectId);
        if (currProj is null)
        {
            return NotFound($"Project with ID {project.ProjectId} not found!. You must provide a valid project to proceed.");
        }
        var user = _userRepository.GetUser(project.UserId);
        if (user is null)
        {
            return NotFound($"User with ID {project.UserId} not found! You must provide a valid user to proceed.");
        }

        // update the project in the database
        try
        {
            _projectRepository.Update(project);
            return Ok(project);
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, $"Database update failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Delete project.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var project = _projectRepository.GetProject(id);
        // check if the project exists
        if (project is null)
        {
            return NotFound($"Project {id} not found!");
        }

        // project cannot be removed if there is any project task attached with it
        if (project.ProjectTasks != null && project.ProjectTasks.Count() > 0)
        {
            return Conflict($"Cannot Delete Project {id}: A project cannot be deleted with active tasks. There are {project.ProjectTasks.Count} active tasks. Delete these tasks before proceeding.)");
        }

        _projectRepository.Delete(id);
        return Ok(project);
    }



}
