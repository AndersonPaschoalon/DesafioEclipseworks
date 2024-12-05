using EclipseTaskManager.Models;
using EclipseTaskManager.Utils;
using System.ComponentModel.DataAnnotations;

namespace EclipseTaskManager.DTOs;

public class ProjectTaskDTO
{
    public int ProjectTaskId { get; set; }

    [Required]
    [StringLength(DbConsts.STR_MEDIUM)]
    public string Title { get; set; }

    [Required]
    [StringLength(DbConsts.STR_BIG)]
    public string Description { get; set; }

    public DateTime? CreationDate { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

    [Required]
    public ProjectTask.ProjectTaskStatus Status { get; set; }

    [Required]
    public ProjectTask.ProjectTaskPriority Priority { get; set; }

    public int UserId { get; set; }
    public int ProjectId { get; set; }
}
