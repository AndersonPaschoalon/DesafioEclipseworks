using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EclipseTaskManager.Models;

namespace EclipseTaskManager.Context;

public class EclipseTaskManagerContext : DbContext
{
    public EclipseTaskManagerContext(DbContextOptions<EclipseTaskManagerContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Project>  Projects { get; set; }
    
    public DbSet<ProjectTask> ProjectTasks { get; set; }

    public DbSet<ProjectTaskComment> ProjectTaskComments { get; set; }

    public DbSet<ProjectTaskUpdate> ProjectTaskUpdates { get; set; }    


}
