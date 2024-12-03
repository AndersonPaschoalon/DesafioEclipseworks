﻿using EclipseTaskManager.Context;
using EclipseTaskManager.Models;
using EclipseTaskManager.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Numerics;

namespace EclipseTaskManager.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectTasksController : ControllerBase
{
    // ActionResult<IEnumerable<ProjectTask>> GetByProject(int projectId)
    // ActionResult<ProjectTask> GetById(int id)
    // ActionResult Post(ProjectTask projectTask)
    // ActionResult Delete(int id)


    private readonly EclipseTaskManagerContext _context;
    private readonly int MaxTaskByProject = 20;

    public ProjectTasksController(EclipseTaskManagerContext context)
    {
        _context = context;
    }


    /// <summary>
    /// Lists all the tasks of a given project.
    /// </summary>
    /// <param name="projectId"></param>
    /// <returns></returns>
    [HttpGet("projectId:int")]
    public ActionResult<IEnumerable<ProjectTask>> GetTaskByProject(int projectId)
    {
        var projectTasks = _context.ProjectTasks.AsNoTracking().Where(t => t.ProjectId == projectId);
        if (projectTasks is null || !projectTasks.Any())
        {
            return NotFound($"No task found for project {projectId}");
        }
        return Ok(projectTasks);
    }


    /// <summary>
    /// Returns a given task given by its id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("id:int", Name = "GetProjectTask")]
    public ActionResult<ProjectTask> GetTaskById(int id)
    {
        var projectTask = _context.ProjectTasks.AsNoTracking()
                                .Include(u => u.Updates)
                                .Include(c => c.Comment)
                                .FirstOrDefault(t => t.ProjectTaskId == id);

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
        var project = _context.Projects.AsNoTracking()
            .Include(p => p.ProjectTasks)
            .FirstOrDefault(p => p.ProjectId == projectTask.ProjectId);
        if (project == null)
        {
            return NotFound($"Project ID {projectTask.ProjectId} not found.");
        }

        var user = _context.Users.AsNoTracking()
            .FirstOrDefault(u => u.UserId == projectTask.UserId);
        if (user == null)
        {
            return NotFound($"User ID {projectTask.UserId} not found.");
        }

        if (project.ProjectTasks == null)
        {
            project.ProjectTasks = new Collection<ProjectTask>();
        }
        if (project.ProjectTasks.Count >= MaxTaskByProject)
        {
            return Forbid($"The limit of {MaxTaskByProject} has been rechead. Delete another task to proceed.");
        }

        // clean-up the incomming data
        projectTask = cleanupIncommingTask(projectTask, DateTime.Now);


        // save new project in the database
        try
        {
            _context.ProjectTasks.Add(projectTask);
            _context.SaveChanges();
            return new CreatedAtRouteResult("GetProjectTask", new { id = projectTask.ProjectTaskId }, projectTask);
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, $"Database update failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Delete a task.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    public ActionResult DeleteTask(int id)
    {

        var projectTask = _context.ProjectTasks.FirstOrDefault(p => p.ProjectTaskId == id);
        if (projectTask is null)
        {
            return NotFound($"Project task {projectTask} not found.");
        }

        _context.ProjectTasks.Remove(projectTask);
        _context.SaveChanges();

        return Ok(projectTask);
    }

    /*
    [HttpPatch("{id:int}")]
    public ActionResult PatchTask(int id, [FromBody]  JsonPatchDocument<ProjectTask> patchDoc)
    {
        // check if the patch and the task id are valid
        if (patchDoc == null)
        {
            return BadRequest("Invalid patch document.");
        }
        var currentTask = _context.ProjectTasks.FirstOrDefault(t => t.ProjectTaskId == id);
        if (currentTask == null)
        {
            return NotFound($"Task {id} not found.");
        }

        // create a copy of the original task
        var originalCopy = new ProjectTask
        {
            ProjectTaskId = currentTask.ProjectTaskId,
            CreationDate = currentTask.CreationDate,
            Description = currentTask.Description,
            DueDate = currentTask.DueDate,
            Priority = currentTask.Priority,
            ProjectId = currentTask.ProjectId,
            UserId = currentTask.UserId,
            Status = currentTask.Status,
            Title = currentTask.Title,
        };

        // apply changes
        patchDoc.ApplyTo(currentTask);

        // helper method to detect the changes, and create a dictionary for it
        var changes = ObjectHelper.DetectChanges(originalCopy, currentTask);
        if (changes.Count > 0)
        {
            var updates = new Collection<ProjectTaskUpdate>();
            foreach (var change in changes)
            {
                updates.Add(new ProjectTaskUpdate {
                    ModifiedField = change.Key,
                    ModificationDate = DateTime.Now,
                    NewFieldValue = change.Value.NewValue,
                    OldFieldValue = change.Value.OldValue, 
                    //Project = currentTask.Project,
                    ProjectId= currentTask.ProjectId,
                    ProjectTask = currentTask,
                    ProjectTaskId = currentTask.ProjectId,
                    //User = currentTask.User,
                    UserId= currentTask.UserId,
                });
            }
            
            // update history table
            _context.ProjectTaskUpdates.AddRange(updates);
            _context.Entry(currentTask).State = EntityState.Modified;
            // save changes
            _context.SaveChanges();
        }

        return Ok(currentTask);
    }
    */


    [HttpPut]
    public ActionResult UpdateTask(ProjectTask updatedTask)
    {
        if (updatedTask == null)
        {
            return BadRequest("Invalid update payload.");
        }

        // Retrieve the current entity from the database
        var currentTask = _context.ProjectTasks.FirstOrDefault(t => t.ProjectTaskId == updatedTask.ProjectTaskId);
        if (currentTask == null)
        {
            return NotFound($"Task with ID {updatedTask.ProjectTaskId} not found.");
        }
        if(currentTask.ProjectId != updatedTask.ProjectId)
        {
            return Conflict($"You are not allowed to change the Project the Task is associated with. Project ID is {currentTask.ProjectId}");
        }
        // but, you may change the user associated with the task.


        // Detect changes between the current task and the updated task
        var changes = ObjectHelper.DetectChanges(currentTask, updatedTask);
        if (changes.Count > 0)
        {
            foreach (var change in changes)
            {
                _context.ProjectTaskUpdates.Add(new ProjectTaskUpdate
                {
                    ModifiedField = change.Key,
                    ModificationDate = DateTime.Now,
                    NewFieldValue = change.Value.NewValue,
                    OldFieldValue = change.Value.OldValue,
                    ProjectId = currentTask.ProjectId,
                    ProjectTaskId = currentTask.ProjectTaskId,
                    UserId = currentTask.UserId,
                });
            }

            // Update the current entity properties
            _context.Entry(currentTask).CurrentValues.SetValues(updatedTask);

            // Save all changes
            _context.SaveChanges();
        }

        return Ok(currentTask);
    }




    private ProjectTask cleanupIncommingTask(ProjectTask projectTask, DateTime? dt) 
    {
        projectTask.Comment = null;
        projectTask.Updates = null;
        projectTask.CreationDate = dt;
        projectTask.Status = ObjectHelper.CastToEnum((int)projectTask.Status, ProjectTask.ProjectTaskStatus.ToDo);
        projectTask.Priority = ObjectHelper.CastToEnum((int)projectTask.Priority, ProjectTask.ProjectTaskPriority.Low);

        return projectTask;
    }

}