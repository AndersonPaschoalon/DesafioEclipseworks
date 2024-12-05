using EclipseTaskManager.Models;

namespace EclipseTaskManager.DTOs.Mappings;

public static class ProjectDTOMappingExtensions
{
    public static ProjectDTO? ToProjectDTO(this Project project)
    {
        if (project is null) return null;

        return new ProjectDTO
        {
            ProjectId = project.ProjectId,
            Title = project.Title,
            Description = project.Description,
            UserId = project.UserId
        };
    }

    public static Project? ToProject(this ProjectDTO projectDto)
    {
        if (projectDto is null) return null;

        return new Project
        {
            ProjectId = projectDto.ProjectId,
            Title = projectDto.Title,
            Description = projectDto.Description,
            UserId = projectDto.UserId
        };
    }

    public static IEnumerable<ProjectDTO> ToProjectDTOList(this IEnumerable<Project> projects)
    {
        if (projects is null || !projects.Any())
        {
            return new List<ProjectDTO>();
        }

        return projects.Select(project => new ProjectDTO
        {
            ProjectId = project.ProjectId,
            Title = project.Title,
            Description = project.Description,
            UserId = project.UserId
        }).ToList();
    }
}
