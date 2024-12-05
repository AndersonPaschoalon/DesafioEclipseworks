using EclipseTaskManager.Utils;
using System.ComponentModel.DataAnnotations;

namespace EclipseTaskManager.DTOs;

public class UserDTO
{
    public int UserId { get; set; }

    [Required]
    [StringLength(DbConsts.STR_MEDIUM)]
    public string Name { get; set; }

    [Required(ErrorMessage = "You must provide a Role Code (Admin:0, Consumer:1)")]
    [Range(0, 1, ErrorMessage = "The Role Code must be between 0 and 1")]
    public int Role { get; set; }

    public string RoleStr { get; set; }
}
