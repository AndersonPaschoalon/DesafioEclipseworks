using EclipseTaskManager.Context;
using EclipseTaskManager.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace EclipseTaskManager.Repository;


public class UserRepository : IUserRepository
{
    private readonly EclipseTaskManagerContext _context;

    public UserRepository(EclipseTaskManagerContext context)
    {
        _context = context;
    }

    public IEnumerable<User> GetUsers()
    {
        return _context.Users.AsNoTracking().Include(u => u.Projects).ToList();
    }

    public User GetUser(int id)
    {
        return _context.Users.AsNoTracking().FirstOrDefault(u => u.UserId == id);
    }

    public User Create(User user)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));

        _context.Users.Add(user);
        _context.SaveChanges();

        return user;
    }

    public User Update(User user)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));

        _context.Entry(user).State = EntityState.Modified;
        _context.SaveChanges();

        return user;
    }

    public User Delete(int id)
    {
        var user = _context.Users.Find(id);

        if (user is null)
            throw new ArgumentNullException(nameof(user));

        _context.Users.Remove(user);
        _context.SaveChanges();

        return user;
    }
}