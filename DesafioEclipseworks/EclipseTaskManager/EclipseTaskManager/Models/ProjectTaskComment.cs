using EclipseTaskManager.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EclipseTaskManager.Models;

[Table("ProjectTaskComments")]
public class ProjectTaskComment
{

    [Key]
    public int ProjectTaskCommentId { get; set; }

    [Required]
    [StringLength(DbConsts.STR_HUGE)]
    public string Comment { get; set; }

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
