using EclipseTaskManager.Models;

namespace EclipseTaskManager.Repository;

public interface IProjectRepository
{
    IEnumerable<Project> GetProjects();
    Project GetProject(int id);
    Project Create(Project project);
    Project Update(Project project);
    Project Delete(int id);
}
