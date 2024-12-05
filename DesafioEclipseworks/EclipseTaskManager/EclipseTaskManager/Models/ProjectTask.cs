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
        Comments = new Collection<ProjectTaskComment>();
        Updates = new Collection<ProjectTaskUpdate>();
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

    [Required(ErrorMessage = "You must provide a title")]
    [StringLength(DbConsts.STR_MEDIUM)]
    public string Title { get; set; }

    [Required(ErrorMessage = "You must provide a description")]
    [StringLength(DbConsts.STR_BIG)]
    public string Description { get; set; }

    public DateTime? CreationDate { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

    [JsonIgnore]
    public DateTime? ConclusionDate { get; set; }

    [Required(ErrorMessage = "You must provide a Status Code (ToDO:0, InProgress:1, Done:2)")]
    [Range(0, 2, ErrorMessage = "The Status Code must be between 0 and 2")]
    public ProjectTaskStatus Status { get; set; }

    // public string StatusStr => Status.ToString();

    [Required(ErrorMessage = "You must provide a a Priority Code (ToDO:0, InProgress:1, Done:2)")]
    [Range(0, 2, ErrorMessage = "The Priority Code must be between 0 and 2")]
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
    public ICollection<ProjectTaskComment>? Comments { get; set; }

    //[JsonIgnore]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ICollection<ProjectTaskUpdate>? Updates { get; set; }
}
