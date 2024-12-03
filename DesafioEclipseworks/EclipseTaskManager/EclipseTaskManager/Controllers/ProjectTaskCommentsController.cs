using EclipseTaskManager.Context;
using EclipseTaskManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EclipseTaskManager.Controllers;

/// <summary>
/// TODO: provavelmente posso remover esse controller.
/// </summary>

[Route("api/[controller]")]
[ApiController]
public class ProjectTaskCommentsController : ControllerBase
{

    private readonly EclipseTaskManagerContext _context;

    public ProjectTaskCommentsController(EclipseTaskManagerContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ProjectTaskComment>> Get()
    {
        var comments = _context.ProjectTaskComments.ToList();
        if (comments is null)
        {
            return NotFound("No comment found");
        }
        return Ok(comments);
    }

    [HttpGet("id:int", Name = "GetComment")]
    public ActionResult<ProjectTaskComment> Get(int id)
    {
        var comment = _context.ProjectTaskComments.FirstOrDefault(p => p.ProjectTaskCommentId == id);
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

        // TODO usar async
        var userExists = _context.Users.Any(u  => u.UserId == comment.UserId);
        var projectExists = _context.Projects.Any(p  => p.ProjectId == comment.ProjectId);
        var taskExists = _context.ProjectTasks.Any(t  => t.ProjectTaskId== comment.ProjectTaskId);

        if (!userExists)
        {
            return NotFound($"UserId {comment.UserId} not found.");
        }
        if (!projectExists)
        {
            return NotFound($"ProjectId{comment.ProjectId} not found.");
        }
        if (!taskExists)
        {
            return NotFound($"TaskId {comment.ProjectTaskId} not found.");
        }

        _context.ProjectTaskComments.Add(comment);
        _context.SaveChanges();

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
    public ActionResult Put(ProjectTaskComment comment)
    {
        if (comment is null)
        {
            return BadRequest("Invalid comment.");
        }
        var commentId = comment.ProjectTaskCommentId;
        var taskComment = _context.ProjectTaskComments.FirstOrDefault(p => p.ProjectTaskCommentId == commentId);
        if (taskComment == null)
        {
            return NotFound($"Comment ID {commentId} not found.");
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

        try
        {
            _context.Entry(comment).State = EntityState.Modified;

            _context.Entry(taskComment).CurrentValues.SetValues(comment);
            _context.SaveChanges();

            return Ok(comment);
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, $"Database update failed: {ex.Message}");
        }
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {

        var comment = _context.ProjectTaskComments.FirstOrDefault(p => p.ProjectTaskCommentId == id);
        if (comment is null)
        {
            return NotFound($"CommentId {id} not found!");
        }

        _context.ProjectTaskComments.Remove(comment);
        _context.SaveChanges();

        return Ok(comment);
    }


}
