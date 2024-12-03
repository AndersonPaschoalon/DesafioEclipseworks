using EclipseTaskManager.Utils;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Text.Json.Serialization;

namespace EclipseTaskManager.Models;

[Table("ProjectTasks")]
public class ProjectTask
{
    public ProjectTask()
    {
        // Comment = new Collection<ProjectTaskComment>();
        // Updates = new Collection<ProjectTaskUpdate>();
        Comment = null;
        Updates = null;
    }

    public enum ProjectTaskStatus
    {
        ToDo = 0,
        InProgress = 1,
        Done = 2
    }

    public enum ProjectTaskPriority
    {
        Low = 0,
        Medium = 1,
        High = 2
    }

    [Key]
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
    public ProjectTaskStatus Status { get; set; }

    // public string StatusStr => Status.ToString();

    [Required]
    public ProjectTaskPriority Priority { get; set; }

    // public string PriorityStr => Priority.ToString();


    // foreign keys
    [Required]
    public int UserId { get; set; }
    [Required]
    public int ProjectId { get; set; }



    // navitation properties

    [JsonIgnore]
    public User? User { get; set; }

    [JsonIgnore]
    public Project? Project { get; set; }

    //[JsonIgnore]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ICollection<ProjectTaskComment>? Comment { get; set; }

    //[JsonIgnore]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ICollection<ProjectTaskUpdate>? Updates { get; set; }
}
