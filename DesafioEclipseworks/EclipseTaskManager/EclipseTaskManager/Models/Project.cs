using EclipseTaskManager.Utils;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EclipseTaskManager.Models;


[Table("Projects")]
public class Project
{
    public Project()
    {
        ProjectTasks = new Collection<ProjectTask>();
    }

    [Key]
    public int ProjectId { get; set; }

    [Required]
    [StringLength(DbConsts.STR_MEDIUM)]
    public string Title { get; set; }

    [Required]
    [StringLength(DbConsts.STR_BIG)]
    public string Description { get; set; }

    // foreign keys
    [Required]
    public int UserId { get; set; }


    // navigation properties

    [JsonIgnore]
    public User? User { get; set; }

    [JsonIgnore]
    public ICollection<ProjectTask>? ProjectTasks { get; set; }  
}
