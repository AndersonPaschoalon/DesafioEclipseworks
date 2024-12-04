using EclipseTaskManager.Context;
using EclipseTaskManager.Models;
using Microsoft.EntityFrameworkCore;

namespace EclipseTaskManager.Repository;


public class ProjectTaskUpdateRepository : IProjectTaskUpdateRepository
{
    private readonly EclipseTaskManagerContext _context;

    public ProjectTaskUpdateRepository(EclipseTaskManagerContext context)
    {
        _context = context;
    }

    public IEnumerable<ProjectTaskUpdate> GetAllUpdates()
    {
        return _context.ProjectTaskUpdates.AsNoTracking().ToList();
    }

    public ProjectTaskUpdate GetUpdateById(int id)
    {
        return _context.ProjectTaskUpdates.AsNoTracking()
            .Include(u => u.User)
            .Include(u => u.Project)
            .Include(u => u.ProjectTask)
            .FirstOrDefault(u => u.ProjectTaskUpdateId == id);
    }

    public ProjectTaskUpdate Create(ProjectTaskUpdate update)
    {
        if (update is null)
            throw new ArgumentNullException(nameof(update));

        _context.ProjectTaskUpdates.Add(update);
        _context.SaveChanges();

        return update;
    }

    public ProjectTaskUpdate Update(ProjectTaskUpdate update)
    {
        if (update is null)
            throw new ArgumentNullException(nameof(update));

        _context.Entry(update).State = EntityState.Modified;
        _context.SaveChanges();

        return update;
    }

    public ProjectTaskUpdate Delete(int id)
    {
        var update = _context.ProjectTaskUpdates.Find(id);

        if (update is null)
            throw new ArgumentNullException(nameof(update));

        _context.ProjectTaskUpdates.Remove(update);
        _context.SaveChanges();

        return update;
    }
}