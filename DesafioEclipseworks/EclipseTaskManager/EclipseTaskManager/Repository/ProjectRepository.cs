using EclipseTaskManager.Context;
using EclipseTaskManager.Models;
using Microsoft.EntityFrameworkCore;
namespace EclipseTaskManager.Repository;


public class ProjectRepository : IProjectRepository
{
    private readonly EclipseTaskManagerContext _context;

    public ProjectRepository(EclipseTaskManagerContext context)
    {
        _context = context;
    }

    public IEnumerable<Project> GetProjects()
    {
        return _context.Projects.AsNoTracking().ToList();
    }

    public Project GetProject(int id)
    {
        return _context.Projects.AsNoTracking()
                .Include(pt => pt.User)
                .Include(pt => pt.ProjectTasks)
                .FirstOrDefault(p => p.ProjectId == id);
    }

    public Project Create(Project project)
    {
        if (project is null)
            throw new ArgumentNullException(nameof(project));

        _context.Projects.Add(project);
        _context.SaveChanges();

        return project;
    }

    public Project Update(Project project)
    {
        if (project is null)
            throw new ArgumentNullException(nameof(project));

        _context.Entry(project).State = EntityState.Modified;
        _context.SaveChanges();

        return project;
    }

    public Project Delete(int id)
    {
        var project = _context.Projects.Find(id);

        if (project is null)
            throw new ArgumentNullException(nameof(project));

        _context.Projects.Remove(project);
        _context.SaveChanges();

        return project;
    }
}