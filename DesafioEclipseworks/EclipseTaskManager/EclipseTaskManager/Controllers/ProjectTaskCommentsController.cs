using EclipseTaskManager.Context;
using EclipseTaskManager.Models;
using EclipseTaskManager.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EclipseTaskManager.Controllers;

/// <summary>
/// TODO: provavelmente posso remover esse controller.
/// </summary>

[Route("v1/api/[controller]")]
[ApiController]
public class ProjectTaskCommentsController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectTaskRepository _taskRepository;
    private readonly IProjectTaskCommentRepository _commentRepository;
    private readonly IProjectTaskUpdateRepository _updateRepository;
    private readonly ILogger _logger;

    public ProjectTaskCommentsController(IUserRepository userRepository,
                                         IProjectRepository projectRepository,
                                         IProjectTaskRepository taskRepository,
                                         IProjectTaskCommentRepository commentRepository,
                                         IProjectTaskUpdateRepository updateRepository,
                                         ILogger<ProjectTaskCommentsController> logger)
    {
        _userRepository = userRepository;
        _projectRepository = projectRepository;
        _taskRepository = taskRepository;
        _commentRepository = commentRepository;
        _updateRepository = updateRepository;
        _logger = logger;
    }

    [HttpGet("all")]
    public ActionResult<IEnumerable<ProjectTaskComment>> GetAll()
    {
        var comments = _commentRepository.GetComments();
        if (comments is null)
        {
            return NotFound("No comment found");
        }
        return Ok(comments);
    }

    [HttpGet("{id:int}", Name = "GetComment")]
    public ActionResult<ProjectTaskComment> Get(int id)
    {
        var comment = _commentRepository.GetComment(id);
        if (comment is null)
        {
            return NotFound($"CommentId {id}");
        }
        return Ok(comment);

    }

    [HttpPost]
    public ActionResult Post(ProjectTaskComment comment)
    {
        if (comment is null)
        {
            return BadRequest("Invalid comment.");
        }

        // validate user: user must exist
        var user = _userRepository.GetUser(comment.UserId);
        if (user is null)
        {
            return NotFound($"UserId {comment.UserId} not found.");
        }

        // validate project: project must exist
        var proj = _projectRepository.GetProject(comment.ProjectId);
        if (proj is null)
        {
            return NotFound($"ProjectId{comment.ProjectId} not found.");
        }


        // validate task: task must exist AND it must point to the same project the comment is pointing
        var projectTask = _taskRepository.GetTask(comment.ProjectTaskId);
        if (projectTask is null)
        {
            return NotFound($"TaskId {comment.ProjectTaskId} not found.");
        }
        if (projectTask.ProjectId != comment.ProjectId)
        {
            return Conflict($"The ProjectTask and the Comment must point to the same Project ID. ProjectTask's project ID is {projectTask.ProjectId}");
        }

        // add comment to history table to
        var historyEntry = new ProjectTaskUpdate
        {
            ModifiedField = nameof(comment.Comment),
            ModificationDate = DateTime.Now,
            NewFieldValue = comment.Comment,
            OldFieldValue = string.Empty,
            ProjectId = comment.ProjectId,
            ProjectTaskId = comment.ProjectTaskId,
            UserId = comment.UserId,
        };

        // update database
        _commentRepository.Create(comment);
        _updateRepository.Create(historyEntry);

        return new CreatedAtRouteResult("GetComment", new { id = comment.ProjectTaskCommentId }, comment);
    }

    /// <summary>
    /// It is not allowed to:
    /// (1) Change the ProjectTask the comment is associated with.
    /// (2) Change the ProjectId the comment is associated with.
    /// (3) Change the UserId the comment is associated with.
    /// </summary>
    /// <param name="comment"></param>
    /// <returns></returns>
    [HttpPut]
    public ActionResult<ProjectTaskComment> Put(ProjectTaskComment comment)
    {
        if (comment is null)
        {
            return BadRequest("Invalid comment.");
        }

        var taskComment = _commentRepository.GetComment(comment.ProjectTaskCommentId);
        if (taskComment == null)
        {
            return NotFound($"Comment ID {comment.ProjectTaskCommentId} not found.");
        }
        if (taskComment.ProjectTaskId != comment.ProjectTaskId)
        {
            return Conflict($"You are not allowed to update Task the comment is associated with. Project ID is {taskComment.ProjectTaskId}");
        }
        if (taskComment.ProjectId != comment.ProjectId)
        {
            return Conflict($"You cannot change the Project the comment is associated with. Project ID is {taskComment.ProjectId}");
        }
        if (taskComment.UserId != comment.UserId)
        {
            return Conflict($"You cannot change the User the comment is associated with. User ID is {taskComment.UserId}");
        }

        if (taskComment.Comment != comment.Comment) 
        {
            try
            {
                var historyEntry = new ProjectTaskUpdate
                {
                    ModifiedField = nameof(comment.Comment),
                    ModificationDate = DateTime.Now,
                    NewFieldValue = comment.Comment,
                    OldFieldValue = taskComment.Comment,
                    ProjectId = taskComment.ProjectId,
                    ProjectTaskId = taskComment.ProjectTaskId,
                    UserId = taskComment.UserId,
                };

                // Update the current entity properties
                _commentRepository.Update(comment);
                _updateRepository.Create(historyEntry);

                return Ok(comment);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Database update failed: {ex.Message}");
            }
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var comment = _commentRepository.GetComment(id);
        if (comment is null)
        {
            return NotFound($"CommentId {id} not found!");
        }
        _commentRepository.Delete(id);
        return Ok(comment);
    }


}
