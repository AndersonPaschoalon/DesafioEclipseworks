using EclipseTaskManager.Models;

namespace EclipseTaskManager.DTOs.Mappings;

public static class UserDTOMappingExtensions
{
    public static UserDTO? ToUserDTO(this User user)
    {
        if (user is null)
            return null;

        return new UserDTO
        {
            UserId = user.UserId,
            Name = user.Name,
            Role = (int)user.Role,
            //RoleStr = user.Role.ToString()
        };
    }

    public static User? ToUser(this UserDTO userDto)
    {
        if (userDto is null)
            return null;

        return new User
        {
            UserId = userDto.UserId,
            Name = userDto.Name,
            Role = (User.UserRole)userDto.Role
        };
    }

    public static IEnumerable<UserDTO> ToUserDTOList(this IEnumerable<User> users)
    {
        if (users is null || !users.Any())
        {
            return new List<UserDTO>();
        }

        return users.Select(user => new UserDTO
        {
            UserId = user.UserId,
            Name = user.Name,
            Role = (int)user.Role,
            //RoleStr = user.Role.ToString()
        }).ToList();
    }
}
