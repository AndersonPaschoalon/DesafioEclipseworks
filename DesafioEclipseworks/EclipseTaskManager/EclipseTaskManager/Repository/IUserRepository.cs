using EclipseTaskManager.Models;

namespace EclipseTaskManager.Repository;

public interface IUserRepository
{
    IEnumerable<User> GetUsers();
    User GetUser(int id);
    User Create(User user);
    User Update(User user);
    User Delete(int id);
}
