﻿using EclipseTaskManager.Utils;
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
        Name = string.Empty;
    }

    [Key]
    public int UserId { get; set; }

    [Required]
    [StringLength(DbConsts.STR_MEDIUM)]
    public string Name { get; set; }

    [Required(ErrorMessage = "You must provide a Role Code (Admin:0, Consumer:1)")]
    [Range(0, 1, ErrorMessage = "The Role Code must be between 0 and 1")]
    public UserRole Role { get; set; }

    public string RoleStr => Role.ToString();

    [JsonIgnore]
    public ICollection<Project>? Projects { get; set; }  
}
