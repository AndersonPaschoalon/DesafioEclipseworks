using EclipseTaskManager.Models;

namespace EclipseTaskManager.DTOs.Mappings;

public static class ProjectTaskUpdateDTOMappingExtensions
{
    public static ProjectTaskUpdateDTO? ToProjectTaskUpdateDTO(this ProjectTaskUpdate update)
    {
        if (update is null) return null;

        return new ProjectTaskUpdateDTO
        {
            ProjectTaskUpdateId = update.ProjectTaskUpdateId,
            ModifiedField = update.ModifiedField,
            OldFieldValue = update.OldFieldValue,
            NewFieldValue = update.NewFieldValue,
            ModificationDate = update.ModificationDate,
            UserId = update.UserId,
            ProjectId = update.ProjectId,
            ProjectTaskId = update.ProjectTaskId
        };
    }

    public static ProjectTaskUpdate? ToProjectTaskUpdate(this ProjectTaskUpdateDTO updateDto)
    {
        if (updateDto is null) return null;

        return new ProjectTaskUpdate
        {
            ProjectTaskUpdateId = updateDto.ProjectTaskUpdateId,
            ModifiedField = updateDto.ModifiedField,
            OldFieldValue = updateDto.OldFieldValue,
            NewFieldValue = updateDto.NewFieldValue,
            ModificationDate = updateDto.ModificationDate,
            UserId = updateDto.UserId,
            ProjectId = updateDto.ProjectId,
            ProjectTaskId = updateDto.ProjectTaskId
        };
    }

    public static IEnumerable<ProjectTaskUpdateDTO> ToProjectTaskUpdateDTOList(this IEnumerable<ProjectTaskUpdate> updates)
    {
        if (updates is null || !updates.Any())
        {
            return new List<ProjectTaskUpdateDTO>();
        }

        return updates.Select(update => new ProjectTaskUpdateDTO
        {
            ProjectTaskUpdateId = update.ProjectTaskUpdateId,
            ModifiedField = update.ModifiedField,
            OldFieldValue = update.OldFieldValue,
            NewFieldValue = update.NewFieldValue,
            ModificationDate = update.ModificationDate,
            UserId = update.UserId,
            ProjectId = update.ProjectId,
            ProjectTaskId = update.ProjectTaskId
        }).ToList();
    }
}


