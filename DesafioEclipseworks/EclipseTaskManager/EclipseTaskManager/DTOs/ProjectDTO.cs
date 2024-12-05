using EclipseTaskManager.Utils;
using System.ComponentModel.DataAnnotations;

namespace EclipseTaskManager.DTOs;

public class ProjectDTO
{
    public int ProjectId { get; set; }

    [Required(ErrorMessage = "You must provide a title")]
    [StringLength(DbConsts.STR_MEDIUM)]
    public string Title { get; set; }

    [Required(ErrorMessage = "You must provide a description")]
    [StringLength(DbConsts.STR_BIG)]
    public string Description { get; set; }

    [Required(ErrorMessage = "You must provide a valid project owner")]
    public int UserId { get; set; }
}
