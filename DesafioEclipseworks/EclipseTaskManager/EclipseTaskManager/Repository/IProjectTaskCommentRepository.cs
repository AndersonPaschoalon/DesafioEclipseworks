using EclipseTaskManager.Models;

namespace EclipseTaskManager.Repository;

public interface IProjectTaskCommentRepository
{
    IEnumerable<ProjectTaskComment> GetComments();
    ProjectTaskComment GetComment(int id);
    ProjectTaskComment Create(ProjectTaskComment comment);
    ProjectTaskComment Update(ProjectTaskComment comment);
    ProjectTaskComment Delete(int id);
}


