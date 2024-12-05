using EclipseTaskManager.Models;
using EclipseTaskManager.Utils;
using System.ComponentModel.DataAnnotations;

namespace EclipseTaskManager.DTOs;

public class ProjectTaskDTOFull
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

    [Required]
    public int UserId { get; set; }

    [Required]
    public int ProjectId { get; set; }

    // Propriedades de navegação

    [Required]
    public UserDTO User { get; set; }

    [Required]
    public ProjectDTO Project { get; set; }

    [Required]
    public ICollection<ProjectTaskCommentDTO> Comments { get; set; }

    [Required]
    public ICollection<ProjectTaskUpdateDTO> Updates { get; set; }
}
