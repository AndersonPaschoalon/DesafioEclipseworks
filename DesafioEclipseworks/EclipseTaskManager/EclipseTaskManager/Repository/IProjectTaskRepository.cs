using EclipseTaskManager.Models;

namespace EclipseTaskManager.Repository;

public interface IProjectTaskRepository
{
    IEnumerable<ProjectTask> GetAllProjectTasks();
    ProjectTask GetProjectTaskById(int id);
    ProjectTask Create(ProjectTask projectTask);
    ProjectTask Update(ProjectTask projectTask);
    ProjectTask Delete(int id);
}

