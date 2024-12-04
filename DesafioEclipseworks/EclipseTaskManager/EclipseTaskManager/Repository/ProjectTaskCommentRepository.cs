using EclipseTaskManager.Context;
using EclipseTaskManager.Models;
using Microsoft.EntityFrameworkCore;

namespace EclipseTaskManager.Repository;

public class ProjectTaskCommentRepository : IProjectTaskCommentRepository
{
    private readonly EclipseTaskManagerContext _context;

    public ProjectTaskCommentRepository(EclipseTaskManagerContext context)
    {
        _context = context;
    }

    public IEnumerable<ProjectTaskComment> GetAllComments()
    {
        return _context.ProjectTaskComments.AsNoTracking().ToList();
    }

    public ProjectTaskComment GetCommentById(int id)
    {
        return _context.ProjectTaskComments.AsNoTracking()
            .Include(c => c.User)
            .Include(c => c.Project)
            .Include(c => c.ProjectTask)
            .FirstOrDefault(c => c.ProjectTaskCommentId == id);
    }

    public ProjectTaskComment Create(ProjectTaskComment comment)
    {
        if (comment is null)
            throw new ArgumentNullException(nameof(comment));

        _context.ProjectTaskComments.Add(comment);
        _context.SaveChanges();

        return comment;
    }

    public ProjectTaskComment Update(ProjectTaskComment comment)
    {
        if (comment is null)
            throw new ArgumentNullException(nameof(comment));

        _context.Entry(comment).State = EntityState.Modified;
        _context.SaveChanges();

        return comment;
    }

    public ProjectTaskComment Delete(int id)
    {
        var comment = _context.ProjectTaskComments.Find(id);

        if (comment is null)
            throw new ArgumentNullException(nameof(comment));

        _context.ProjectTaskComments.Remove(comment);
        _context.SaveChanges();

        return comment;
    }
}