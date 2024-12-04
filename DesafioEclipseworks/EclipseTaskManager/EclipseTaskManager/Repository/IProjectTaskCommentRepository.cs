using EclipseTaskManager.Models;

namespace EclipseTaskManager.Repository;

public interface IProjectTaskCommentRepository
{
    IEnumerable<ProjectTaskComment> GetAllComments();
    ProjectTaskComment GetCommentById(int id);
    ProjectTaskComment Create(ProjectTaskComment comment);
    ProjectTaskComment Update(ProjectTaskComment comment);
    ProjectTaskComment Delete(int id);
}


