using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EclipseTaskManager.Migrations
{
    /// <inheritdoc />
    public partial class DummyData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("INSERT INTO Users (Name, Role) VALUES ('Consumer Default', 0);");
            mb.Sql("INSERT INTO Users (Name, Role) VALUES ('Admin Default', 1);");

            mb.Sql("INSERT INTO Projects (Title, Description, UserId) VALUES ('Projeto 01', 'Projeto 01', 1);");
            mb.Sql("INSERT INTO Projects (Title, Description, UserId) VALUES ('Projeto 02', 'Projeto 02', 2);");

            mb.Sql("INSERT INTO ProjectTasks (Title, Description, CreationDate, DueDate, Status, Priority, UserId, ProjectId) VALUES ('(Proj01) Task 01', 'Task 01', now(), DATE_ADD(NOW(), INTERVAL 10 DAY), 0, 0, 1, 1);");
            mb.Sql("INSERT INTO ProjectTasks (Title, Description, CreationDate, DueDate, Status, Priority, UserId, ProjectId) VALUES ('(Proj01) Task 02', 'Task 02', now(), DATE_ADD(NOW(), INTERVAL 10 DAY), 0, 1, 1, 1);");
            mb.Sql("INSERT INTO ProjectTasks (Title, Description, CreationDate, DueDate, Status, Priority, UserId, ProjectId) VALUES ('(Proj01) Task 01', 'Task 01', now(), DATE_ADD(NOW(), INTERVAL 10 DAY), 0, 2, 2, 2);");

            mb.Sql("INSERT INTO ProjectTaskComments (Comment, UserId, ProjectId, ProjectTaskId) VALUES ('Comment A', 1, 1, 1);");
            mb.Sql("INSERT INTO ProjectTaskComments (Comment, UserId, ProjectId, ProjectTaskId) VALUES ('Comment B', 1, 1, 2);");
            mb.Sql("INSERT INTO ProjectTaskComments (Comment, UserId, ProjectId, ProjectTaskId) VALUES ('Comment C', 2, 2, 1);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("DELETE FROM Projects");
            mb.Sql("DELETE FROM ProjectTasks");
            mb.Sql("DELETE FROM ProjectTaskComments");
            mb.Sql("DELETE FROM ProjectsTaskUpdates");
            mb.Sql("DELETE FROM Users");
        }
    }
}
