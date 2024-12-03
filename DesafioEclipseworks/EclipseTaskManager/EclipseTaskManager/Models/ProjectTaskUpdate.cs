using EclipseTaskManager.Utils;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EclipseTaskManager.Models;

public class ProjectTaskUpdate
{

    [Key]
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

    // foreign keys
    public int UserId { get; set; }
    public int ProjectId { get; set; }
    public int ProjectTaskId { get; set; }


    // navigation properties

    [JsonIgnore]
    public User? User { get; set; }

    [JsonIgnore]
    public Project? Project { get; set; }

    [JsonIgnore]
    public ProjectTask? ProjectTask { get; set; }
}
