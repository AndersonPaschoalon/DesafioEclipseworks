using EclipseTaskManager.DTOs;
using EclipseTaskManager.Models;
using EclipseTaskManager.DTOs.Mappings;

namespace EclipseTaskManager.Unit;


[TestFixture]
public class UserDTOMappingTests
{
    [Test]
    public void ToUserDTO_ShouldMapUserToUserDTO()
    {
        // Arrange
        var user = new User
        {
            UserId = 1,
            Name = "John Doe",
            Role = User.UserRole.Admin
        };

        // Act
        var userDTO = user.ToUserDTO();

        // Assert
        Assert.IsNotNull(userDTO);
        Assert.AreEqual(user.UserId, userDTO.UserId);
        Assert.AreEqual(user.Name, userDTO.Name);
        Assert.AreEqual((int)user.Role, userDTO.Role);
        Assert.AreEqual(user.Role.ToString(), userDTO.RoleStr);
    }

    [Test]
    public void ToUserDTO_ShouldReturnNull_WhenUserIsNull()
    {
        // Act
        var userDTO = ((User)null).ToUserDTO();

        // Assert
        Assert.IsNull(userDTO);
    }

    [Test]
    public void ToUser_ShouldMapUserDTOToUser()
    {
        // Arrange
        var userDTO = new UserDTO
        {
            UserId = 1,
            Name = "John Doe",
            Role = (int)User.UserRole.Admin
        };

        // Act
        var user = userDTO.ToUser();

        // Assert
        Assert.IsNotNull(user);
        Assert.AreEqual(userDTO.UserId, user.UserId);
        Assert.AreEqual(userDTO.Name, user.Name);
        Assert.AreEqual((User.UserRole)userDTO.Role, user.Role);
    }

    [Test]
    public void ToUser_ShouldReturnNull_WhenUserDTOIsNull()
    {
        // Act
        var user = ((UserDTO)null).ToUser();

        // Assert
        Assert.IsNull(user);
    }

    [Test]
    public void ToUserDTOList_ShouldMapListOfUsersToListOfUserDTOs()
    {
        var users = new List<User>
            {
                new User { UserId = 1, Name = "John Doe", Role = User.UserRole.Admin },
                new User { UserId = 2, Name = "Jane Smith", Role = User.UserRole.Consumer }
            };

        var userDTOList = users.ToUserDTOList().ToList();

        Assert.IsNotNull(userDTOList);
        Assert.AreEqual(users.Count, userDTOList.Count);
        Assert.AreEqual(users[0].UserId, userDTOList[0].UserId);
        Assert.AreEqual(users[1].Name, userDTOList[1].Name);
    }

    [Test]
    public void ToUserDTOList_ShouldReturnEmptyList_WhenUsersIsNull()
    {
        var userDTOList = ((IEnumerable<User>)null).ToUserDTOList().ToList();
        Assert.IsNotNull(userDTOList);
        Assert.AreEqual(0, userDTOList.Count);
    }

    [Test]
    public void ToUserDTOList_ShouldReturnEmptyList_WhenUsersIsEmpty()
    {
        var userDTOList = new List<User>().ToUserDTOList().ToList();
        Assert.IsNotNull(userDTOList);
        Assert.AreEqual(0, userDTOList.Count);
    }
}
