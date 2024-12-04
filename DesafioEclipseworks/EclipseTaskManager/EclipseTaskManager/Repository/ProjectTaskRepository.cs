using EclipseTaskManager.Context;
using EclipseTaskManager.Models;
using Microsoft.EntityFrameworkCore;

namespace EclipseTaskManager.Repository;

public class ProjectTaskRepository : IProjectTaskRepository
{
    private readonly EclipseTaskManagerContext _context;

    public ProjectTaskRepository(EclipseTaskManagerContext context)
    {
        _context = context;
    }

    public IEnumerable<ProjectTask> GetAllProjectTasks()
    {
        return _context.ProjectTasks.AsNoTracking().ToList();
    }

    public ProjectTask GetProjectTaskById(int id)
    {
        return _context.ProjectTasks.AsNoTracking()
            .Include(pt => pt.User)
            .Include(pt => pt.Project)
            .Include(pt => pt.Comments)
            .Include(pt => pt.Updates)
            .FirstOrDefault(pt => pt.ProjectTaskId == id);
    }

    public ProjectTask Create(ProjectTask projectTask)
    {
        if (projectTask is null)
            throw new ArgumentNullException(nameof(projectTask));

        _context.ProjectTasks.Add(projectTask);
        _context.SaveChanges();

        return projectTask;
    }

    public ProjectTask Update(ProjectTask projectTask)
    {
        if (projectTask is null)
            throw new ArgumentNullException(nameof(projectTask));

        _context.Entry(projectTask).State = EntityState.Modified;
        _context.SaveChanges();

        return projectTask;
    }

    public ProjectTask Delete(int id)
    {
        var projectTask = _context.ProjectTasks.Find(id);

        if (projectTask is null)
            throw new ArgumentNullException(nameof(projectTask));

        _context.ProjectTasks.Remove(projectTask);
        _context.SaveChanges();

        return projectTask;
    }
}

