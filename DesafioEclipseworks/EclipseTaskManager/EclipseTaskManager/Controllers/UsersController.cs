using EclipseTaskManager.Context;
using EclipseTaskManager.Models;
using EclipseTaskManager.Repository;
using EclipseTaskManager.Utils;
using EclipseTaskManager.DTOs;
using EclipseTaskManager.DTOs.Mappings;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EclipseTaskManager.Controllers;

[Route("v1/api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    // ActionResult<IEnumerable<User>> Get()
    // ActionResult<User> Get(int id)
    // ActionResult Post(int id, User user)
    // ActionResult Put(int id, User user)
    // ActionResult Delete(int id)

    private readonly IUserRepository _userRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ILogger _logger;

    public UsersController(IUserRepository userRepository, 
        IProjectRepository projectRepository, 
        ILogger<UsersController> logger)
    {
        _userRepository = userRepository;
        _projectRepository = projectRepository;
        _logger = logger;
    }

    /// <summary>
    /// Returns the list of all users.
    /// </summary>
    /// <returns></returns>
    [HttpGet("all")]
    public ActionResult<IEnumerable<UserDTO>> GetAll()
    {
        var users = _userRepository.GetUsers();
        if (users == null || !users.Any())
        {
            return NotFound("No users found.");
        }
        return Ok(users.ToUserDTOList());
    }

    /// <summary>
    /// Return a user by its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}", Name = "GetUser")]
    public ActionResult<UserDTO> Get(int id)
    {
        var user = _userRepository.GetUser(id);
        if (user == null)
        {
            return NotFound($"UserId {id} not found.");
        }
        return Ok(user.ToUserDTO());
    }

    /// <summary>
    /// Create a new user.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPost]
    public ActionResult<UserDTO> Post(UserDTO userDto)
    {
        if (userDto == null)
        {
            return BadRequest("Invalid user data.");
        }
        var user = userDto.ToUser();
        user.Role = ObjectHelper.CastToEnum((int)user.Role, Models.User.UserRole.Consumer);
        try
        {
            _userRepository.Create(user);
            return CreatedAtRoute("GetUser", new { id = user.UserId }, user.ToUserDTO());
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, $"Database update failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Update an user. Only an Admin user may execute this operation. Its identification is passed
    /// as query argument.
    /// </summary>
    /// <param name="id">User ID who is performing the operation. Must be an admin.</param>
    /// <param name="user">User to be updated.</param>
    /// <returns></returns>
    [HttpPut]
    public ActionResult<UserDTO> Put(UserDTO userDto)
    {
        // check if the user parameter data is ok
        if (userDto == null)
        {
            return BadRequest("Invalid user data.");
        }
        var user = userDto.ToUser();
        // update the user
        try
        {
            _userRepository.Update(user);
            return Ok(user.ToUserDTO());
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, $"Database update failed: {ex.Message}");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    public ActionResult<UserDTO> Delete(int id)
    {
        var user = _userRepository.GetUser(id);
        if (user == null)
        {
            return NotFound($"UserId {id} not found.");
        }


        // usuario não pode ter projetos em seu nome
        var projects = _projectRepository.GetProjects().Where(p => p.UserId == id);
        if (projects != null && projects.Count() > 0)
        {
            return Conflict($"Cannot remove user with ID {id}. This user has {projects.Count()} projects assigned to. Delete or change the projects owner first.");
        }

        try
        {
            _userRepository.Delete(id);
            return Ok(user.ToUserDTO());
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, $"Database update failed: {ex.Message}");
        }
    }



}
