using EclipseTaskManager.Models;

namespace EclipseTaskManager.DTOs.Mappings;

public static class ProjectTaskDTOMappingExtensions
{
    public static ProjectTaskDTO ToProjectTaskDTO(this ProjectTask task)
    {
        if (task is null) return null;

        return new ProjectTaskDTO
        {
            ProjectTaskId = task.ProjectTaskId,
            Title = task.Title,
            Description = task.Description,
            CreationDate = task.CreationDate,
            DueDate = task.DueDate,
            Status = task.Status,
            Priority = task.Priority,
            UserId = task.UserId,
            ProjectId = task.ProjectId
        };
    }

    public static ProjectTaskDTOFull ToProjectTaskDTOFull(this ProjectTask task)
    {
        if (task is null) return null;

        return new ProjectTaskDTOFull
        {
            ProjectTaskId = task.ProjectTaskId,
            Title = task.Title,
            Description = task.Description,
            CreationDate = task.CreationDate,
            DueDate = task.DueDate,
            Status = task.Status,
            Priority = task.Priority,
            UserId = task.UserId,
            ProjectId = task.ProjectId,
            User = task.User?.ToUserDTO(),
            Project = task.Project?.ToProjectDTO(),
            Comments = task.Comments?.Select(c => c.ToProjectTaskCommentDTO()).ToList(),
            Updates = task.Updates?.Select(u => u.ToProjectTaskUpdateDTO()).ToList()
        };
    }

    public static IEnumerable<ProjectTaskDTO> ToProjectTaskDTOList(this IEnumerable<ProjectTask> tasks)
    {
        if (tasks is null || !tasks.Any()) return new List<ProjectTaskDTO>();

        return tasks.Select(task => task.ToProjectTaskDTO()).ToList();
    }

    public static IEnumerable<ProjectTaskDTOFull> ToProjectTaskDTOFullList(this IEnumerable<ProjectTask> tasks)
    {
        if (tasks is null || !tasks.Any()) return new List<ProjectTaskDTOFull>();

        return tasks.Select(task => task.ToProjectTaskDTOFull()).ToList();
    }
}
