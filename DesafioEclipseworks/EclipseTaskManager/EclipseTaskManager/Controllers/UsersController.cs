using EclipseTaskManager.Context;
using EclipseTaskManager.Models;
using EclipseTaskManager.Utils;
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

    private readonly EclipseTaskManagerContext _context;

    public UsersController(EclipseTaskManagerContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Returns the list of all users.
    /// </summary>
    /// <returns></returns>
    [HttpGet("all")]
    public ActionResult<IEnumerable<User>> GetAll()
    {
        var users = _context.Users.AsNoTracking().ToList();
        if (users == null || !users.Any())
        {
            return NotFound("No users found.");
        }
        return Ok(users);
    }

    /// <summary>
    /// Return a user by its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}", Name = "GetUser")]
    public ActionResult<User> Get(int id)
    {
        var user = _context.Users.AsNoTracking().FirstOrDefault(u => u.UserId == id);
        if (user == null)
        {
            return NotFound($"UserId {id} not found.");
        }
        return Ok(user);
    }

    /// <summary>
    /// Create a new user.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPost]
    public ActionResult Post(User user)
    {
        if (user == null)
        {
            return BadRequest("Invalid user data.");
        }
       
        user.Role = ObjectHelper.CastToEnum((int)user.Role, Models.User.UserRole.Consumer);
        try
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return CreatedAtRoute("GetUser", new { id = user.UserId }, user);
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
    public ActionResult<User> Put(User user)
    {
        // check if the user parameter data is ok
        if (user == null)
        {
            return BadRequest("Invalid user data.");
        }
        // update the user
        try
        {
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(user);
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
    public ActionResult Delete(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.UserId == id);
        if (user == null)
        {
            return NotFound($"UserId {id} not found.");
        }

        // usuario não pode ter projetos em seu nome
        var projects = _context.Projects.AsNoTracking().Where(p => p.UserId == id);
        if (projects != null && projects.Count() > 0)
        {
            return Conflict($"Cannot remove user with ID {id}. This user has {projects.Count()} projects assigned to. Delete or change the projects owner first.");
        }

        try
        {
            _context.Users.Remove(user);
            _context.SaveChanges();

            return Ok(user);
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, $"Database update failed: {ex.Message}");
        }
    }



}
