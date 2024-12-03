using EclipseTaskManager.Utils;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EclipseTaskManager.Models;

[Table("Users")]
public class User
{
    public enum UserRole
    {
        Admin = 0,
        Consumer = 1
    }

    public User()
    {
        //Projects = new Collection<Project>();
        Name = string.Empty;
    }

    [Key]
    public int UserId { get; set; }

    [Required]
    [StringLength(DbConsts.STR_MEDIUM)]
    public string Name { get; set; }

    [Required]
    public UserRole Role { get; set; }

    public string RoleStr => Role.ToString();

    [JsonIgnore]
    public ICollection<Project>? Projects { get; set; }  
}
