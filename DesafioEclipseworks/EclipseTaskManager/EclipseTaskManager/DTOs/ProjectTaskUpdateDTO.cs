using EclipseTaskManager.Utils;
using System.ComponentModel.DataAnnotations;

namespace EclipseTaskManager.DTOs;

public class ProjectTaskUpdateDTO
{
    public int ProjectTaskUpdateId { get; set; }

    [Required]
    [StringLength(DbConsts.STR_SMALL)]
    public string ModifiedField { get; set; }

    [Required]
    [StringLength(DbConsts.STR_BIG)]
    public string OldFieldValue { get; set; }

    [Required]
    [StringLength(DbConsts.STR_BIG)]
    public string NewFieldValue { get; set; }

    [Required]
    public DateTime ModificationDate { get; set; }

    public int UserId { get; set; }
    public int ProjectId { get; set; }
    public int ProjectTaskId { get; set; }
}
