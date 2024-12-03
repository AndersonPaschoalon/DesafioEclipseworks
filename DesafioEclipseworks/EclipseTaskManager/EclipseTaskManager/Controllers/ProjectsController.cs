using EclipseTaskManager.Context;
using EclipseTaskManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace EclipseTaskManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        // ActionResult<IEnumerable<Project>> GetByUser(int userId)
        // ActionResult<Project> GetById(int id)
        // ActionResult Post(Project project)
        // ActionResult Put(Project project)
        // ActionResult Delete(int id)

        private readonly EclipseTaskManagerContext _context;

        public ProjectsController(EclipseTaskManagerContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Returns all projects of a given user specified by its user ID.
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>List of projects its user owns</returns>
        [HttpGet("userId:int")]
        public ActionResult<IEnumerable<Project>> GetByUser(int userId)
        {
            // check if the user exists
            var user = _context.Users.AsNoTracking().FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                return NotFound($"UserId {userId} not found.");
            }

            // retrive the user projects
            var projects = _context.Projects
                .Where(p => p.UserId == userId)
                .ToList();
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
        [HttpGet("id:int", Name = "GetProject")]
        public ActionResult<Project> GetById(int id)
        {
            var project = _context.Projects.AsNoTracking().FirstOrDefault(p => p.ProjectId == id);
            if (project is null)
            {
                return NotFound();
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
            var user = _context.Users.AsNoTracking().FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                return NotFound($"UserId {userId} not found.");
            }

            // save new project in the database
            try 
            {
                _context.Projects.Add(project);
                _context.SaveChanges();
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
            var proj = _context.Projects.AsNoTracking().FirstOrDefault(p => p.ProjectId == project.ProjectId);
            if (proj is null)
            {
                return NotFound($"Project with ID {project.ProjectId} not found!. You must provide a valid project to proceed.");
            }
            var user = _context.Users.AsNoTracking().FirstOrDefault(u => u.UserId == project.UserId);
            if (user is null)
            {
                return NotFound($"User with ID {project.UserId} not found! You must provide a valid user to proceed.");
            }


            // update the project in the database
            try
            {
                _context.Entry(project).State = EntityState.Modified;
                _context.SaveChanges();

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
            // check if the project exists
            var project = _context.Projects.FirstOrDefault(p => p.ProjectId == id);
            if (project is null)
            {
                return NotFound($"Project {id} not found!");
            }

            // project cannot be removed if there is any project task attached with it
            if (project.ProjectTasks != null && project.ProjectTasks.Count() > 0)
            {
                return Forbid($"Cannot Delete Project {id}: A project cannot be deleted with active tasks. There are {project.ProjectTasks.Count} active tasks. Delete these tasks before proceeding.)");
            }

            _context.Projects.Remove(project);
            _context.SaveChanges();

            return Ok(project);
        }



    }
}
