using EclipseTaskManager.Models;

namespace EclipseTaskManager.DTOs.Mappings;

public static class ProjectTaskDTOMappingExtensions
{

    // Model ==> DTO

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

    // DTO ==> Model

    public static ProjectTask ToProjectTask(this ProjectTaskDTO taskDTO)
    {
        if (taskDTO is null) return null;

        return new ProjectTask
        {
            ProjectTaskId = taskDTO.ProjectTaskId,
            Title = taskDTO.Title,
            Description = taskDTO.Description,
            CreationDate = taskDTO.CreationDate,
            DueDate = taskDTO.DueDate,
            Status = taskDTO.Status,
            Priority = taskDTO.Priority,
            UserId = taskDTO.UserId,
            ProjectId = taskDTO.ProjectId
        };
    }

    // Mapeamento de ProjectTaskDTOFull para ProjectTask
    public static ProjectTask ToProjectTask(this ProjectTaskDTOFull taskDTOFull)
    {
        if (taskDTOFull is null) return null;

        var task = new ProjectTask
        {
            ProjectTaskId = taskDTOFull.ProjectTaskId,
            Title = taskDTOFull.Title,
            Description = taskDTOFull.Description,
            CreationDate = taskDTOFull.CreationDate,
            DueDate = taskDTOFull.DueDate,
            Status = taskDTOFull.Status,
            Priority = taskDTOFull.Priority,
            UserId = taskDTOFull.UserId,
            ProjectId = taskDTOFull.ProjectId
        };

        // Mapear as propriedades de navegação, se existirem
        if (taskDTOFull.User != null)
            task.User = taskDTOFull.User.ToUser();

        if (taskDTOFull.Project != null)
            task.Project = taskDTOFull.Project.ToProject();

        if (taskDTOFull.Comments != null)
            task.Comments = taskDTOFull.Comments.Select(c => c.ToProjectTaskComment()).ToList();

        if (taskDTOFull.Updates != null)
            task.Updates = taskDTOFull.Updates.Select(u => u.ToProjectTaskUpdate()).ToList();

        return task;
    }

    // Mapeamento de lista de ProjectTaskDTO para lista de ProjectTask
    public static IEnumerable<ProjectTask> ToProjectTaskList(this IEnumerable<ProjectTaskDTO> taskDTOs)
    {
        if (taskDTOs is null || !taskDTOs.Any()) return new List<ProjectTask>();

        return taskDTOs.Select(taskDTO => taskDTO.ToProjectTask()).ToList();
    }

    // Mapeamento de lista de ProjectTaskDTOFull para lista de ProjectTask
    public static IEnumerable<ProjectTask> ToProjectTaskFullList(this IEnumerable<ProjectTaskDTOFull> taskDTOFulls)
    {
        if (taskDTOFulls is null || !taskDTOFulls.Any()) return new List<ProjectTask>();

        return taskDTOFulls.Select(taskDTOFull => taskDTOFull.ToProjectTask()).ToList();
    }

}
