using EclipseTaskManager.Models;

namespace EclipseTaskManager.DTOs.Mappings;

public static class ProjectTaskCommentDTOMappingExtensions
{
    public static ProjectTaskCommentDTO? ToProjectTaskCommentDTO(this ProjectTaskComment comment)
    {
        if (comment is null) return null;

        return new ProjectTaskCommentDTO
        {
            ProjectTaskCommentId = comment.ProjectTaskCommentId,
            Comment = comment.Comment,
            UserId = comment.UserId,
            ProjectId = comment.ProjectId,
            ProjectTaskId = comment.ProjectTaskId
        };
    }

    public static ProjectTaskComment? ToProjectTaskComment(this ProjectTaskCommentDTO commentDto)
    {
        if (commentDto is null) return null;

        return new ProjectTaskComment
        {
            ProjectTaskCommentId = commentDto.ProjectTaskCommentId,
            Comment = commentDto.Comment,
            UserId = commentDto.UserId,
            ProjectId = commentDto.ProjectId,
            ProjectTaskId = commentDto.ProjectTaskId
        };
    }

    public static IEnumerable<ProjectTaskCommentDTO> ToProjectTaskCommentDTOList(this IEnumerable<ProjectTaskComment> comments)
    {
        if (comments is null || !comments.Any())
        {
            return new List<ProjectTaskCommentDTO>();
        }

        return comments.Select(comment => new ProjectTaskCommentDTO
        {
            ProjectTaskCommentId = comment.ProjectTaskCommentId,
            Comment = comment.Comment,
            UserId = comment.UserId,
            ProjectId = comment.ProjectId,
            ProjectTaskId = comment.ProjectTaskId
        }).ToList();
    }
}
