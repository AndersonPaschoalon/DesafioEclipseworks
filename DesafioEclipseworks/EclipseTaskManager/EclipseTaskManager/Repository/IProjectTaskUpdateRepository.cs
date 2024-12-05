using EclipseTaskManager.Models;

namespace EclipseTaskManager.Repository;

public interface IProjectTaskUpdateRepository
{
    IEnumerable<ProjectTaskUpdate> GetUpdates();
    ProjectTaskUpdate GetUpdate(int id);
    ProjectTaskUpdate Create(ProjectTaskUpdate update);
    ProjectTaskUpdate Update(ProjectTaskUpdate update);
    ProjectTaskUpdate Delete(int id);
}