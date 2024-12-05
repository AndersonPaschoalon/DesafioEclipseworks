using EclipseTaskManager.Models;

namespace EclipseTaskManager.Repository;

public interface IProjectTaskRepository
{
    IEnumerable<ProjectTask> GetTasks();
    ProjectTask GetTask(int id);
    ProjectTask Create(ProjectTask projectTask);
    ProjectTask Update(ProjectTask projectTask);
    ProjectTask Delete(int id);
}

