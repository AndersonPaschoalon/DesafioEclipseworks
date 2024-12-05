using EclipseTaskManager.Utils;
using System.ComponentModel.DataAnnotations;

namespace EclipseTaskManager.DTOs;

public class ProjectTaskCommentDTO
{
    public int ProjectTaskCommentId { get; set; }

    [Required]
    [StringLength(DbConsts.STR_HUGE)]
    public string Comment { get; set; }

    public int UserId { get; set; }
    public int ProjectId { get; set; }
    public int ProjectTaskId { get; set; }
}
