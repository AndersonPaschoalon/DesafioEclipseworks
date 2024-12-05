namespace EclipseTaskManager.Unit.DTOsTests;

using EclipseTaskManager.DTOs;
using EclipseTaskManager.Models;
using EclipseTaskManager.DTOs.Mappings;


[TestFixture]
public class ProjectTaskDTOMappingTests
{
    [Test]
    public void ToProjectTaskDTO_ShouldConvertProjectTaskToDTO()
    {
        // Arrange
        var task = new ProjectTask
        {
            ProjectTaskId = 1,
            Title = "Test Task",
            Description = "Test Description",
            CreationDate = DateTime.Now,
            DueDate = DateTime.Now.AddDays(1),
            Status = ProjectTask.ProjectTaskStatus.ToDo,
            Priority = ProjectTask.ProjectTaskPriority.Medium,
            UserId = 100,
            ProjectId = 200
        };

        // Act
        var taskDTO = task.ToProjectTaskDTO();

        // Assert
        Assert.NotNull(taskDTO);
        Assert.AreEqual(task.ProjectTaskId, taskDTO.ProjectTaskId);
        Assert.AreEqual(task.Title, taskDTO.Title);
        Assert.AreEqual(task.Description, taskDTO.Description);
        Assert.AreEqual(task.CreationDate, taskDTO.CreationDate);
        Assert.AreEqual(task.DueDate, taskDTO.DueDate);
        Assert.AreEqual(task.Status, taskDTO.Status);
        Assert.AreEqual(task.Priority, taskDTO.Priority);
        Assert.AreEqual(task.UserId, taskDTO.UserId);
        Assert.AreEqual(task.ProjectId, taskDTO.ProjectId);
    }

    [Test]
    public void ToProjectTask_ShouldConvertDTOToProjectTask()
    {
        // Arrange
        var taskDTO = new ProjectTaskDTO
        {
            ProjectTaskId = 1,
            Title = "Test Task",
            Description = "Test Description",
            CreationDate = DateTime.Now,
            DueDate = DateTime.Now.AddDays(1),
            Status = ProjectTask.ProjectTaskStatus.ToDo,
            Priority = ProjectTask.ProjectTaskPriority.Medium,
            UserId = 100,
            ProjectId = 200
        };

        // Act
        var task = taskDTO.ToProjectTask();

        // Assert
        Assert.NotNull(task);
        Assert.AreEqual(taskDTO.ProjectTaskId, task.ProjectTaskId);
        Assert.AreEqual(taskDTO.Title, task.Title);
        Assert.AreEqual(taskDTO.Description, task.Description);
        Assert.AreEqual(taskDTO.CreationDate, task.CreationDate);
        Assert.AreEqual(taskDTO.DueDate, task.DueDate);
        Assert.AreEqual(taskDTO.Status, task.Status);
        Assert.AreEqual(taskDTO.Priority, task.Priority);
        Assert.AreEqual(taskDTO.UserId, task.UserId);
        Assert.AreEqual(taskDTO.ProjectId, task.ProjectId);
    }

    [Test]
    public void ToProjectTaskDTOFull_ShouldConvertProjectTaskToFullDTO()
    {
        // Arrange
        var task = new ProjectTask
        {
            ProjectTaskId = 1,
            Title = "Test Task",
            Description = "Test Description",
            CreationDate = DateTime.Now,
            DueDate = DateTime.Now.AddDays(1),
            Status = ProjectTask.ProjectTaskStatus.ToDo,
            Priority = ProjectTask.ProjectTaskPriority.Medium,
            UserId = 100,
            ProjectId = 200,
            Comments = new List<ProjectTaskComment>
            {
                new ProjectTaskComment { 
                    ProjectTaskCommentId = 1,
                    ProjectId = 1,
                    ProjectTaskId = 1,
                    UserId = 100,
                    Comment = "Test comment" 
                },
                new ProjectTaskComment {
                    ProjectTaskCommentId = 2,
                    ProjectId = 2,
                    ProjectTaskId = 2,
                    UserId = 200,
                    Comment = "Test comment"
                }
            },
            Updates = new List<ProjectTaskUpdate>
            {
                new ProjectTaskUpdate { 
                    ProjectTaskUpdateId = 1, 
                    ModifiedField = "Field", 
                    OldFieldValue = "Old", 
                    NewFieldValue = "New", 
                    ModificationDate = DateTime.Now 
                },
                new ProjectTaskUpdate {
                    ProjectTaskUpdateId = 2,
                    ModifiedField = "Field",
                    OldFieldValue = "Old",
                    NewFieldValue = "New",
                    ModificationDate = DateTime.Now
                }
            }
        };

        // Act
        var taskDTOFull = task.ToProjectTaskDTOFull();

        // Assert
        Assert.NotNull(taskDTOFull);
        Assert.AreEqual(task.ProjectTaskId, taskDTOFull.ProjectTaskId);
        Assert.AreEqual(task.Title, taskDTOFull.Title);
        Assert.AreEqual(task.Description, taskDTOFull.Description);
        Assert.AreEqual(task.CreationDate, taskDTOFull.CreationDate);
        Assert.AreEqual(task.DueDate, taskDTOFull.DueDate);
        Assert.AreEqual(task.Status, taskDTOFull.Status);
        Assert.AreEqual(task.Priority, taskDTOFull.Priority);
        Assert.AreEqual(task.UserId, taskDTOFull.UserId);
        Assert.AreEqual(task.ProjectId, taskDTOFull.ProjectId);
        Assert.AreEqual(task.Comments.Count, taskDTOFull.Comments.Count);
        Assert.AreEqual(task.Updates.Count, taskDTOFull.Updates.Count);
    }

    [Test]
    public void ToProjectTaskDTOList_ShouldConvertListOfTasksToListOfDTOs()
    {
        // Arrange
        var tasks = new List<ProjectTask>
        {
            new ProjectTask { ProjectTaskId = 1, Title = "Task 1", Description = "Description 1", DueDate = DateTime.Now.AddDays(1), Status = ProjectTask.ProjectTaskStatus.ToDo, Priority = ProjectTask.ProjectTaskPriority.Low, UserId = 1, ProjectId = 1 },
            new ProjectTask { ProjectTaskId = 2, Title = "Task 2", Description = "Description 2", DueDate = DateTime.Now.AddDays(2), Status = ProjectTask.ProjectTaskStatus.InProgress, Priority = ProjectTask.ProjectTaskPriority.High, UserId = 2, ProjectId = 2 }
        };

        // Act
        var taskDTOs = tasks.ToProjectTaskDTOList().ToList();

        // Assert
        Assert.NotNull(taskDTOs);
        Assert.AreEqual(tasks.Count, taskDTOs.Count);
        Assert.AreEqual(tasks.First().ProjectTaskId, taskDTOs.First().ProjectTaskId);
        Assert.AreEqual(tasks.Last().ProjectTaskId, taskDTOs.Last().ProjectTaskId);
    }

    [Test]
    public void ToProjectTaskList_ShouldConvertListOfDTOsToListOfTasks()
    {
        // Arrange
        var taskDTOs = new List<ProjectTaskDTO>
        {
            new ProjectTaskDTO { ProjectTaskId = 1, Title = "Task 1", Description = "Description 1", DueDate = DateTime.Now.AddDays(1), Status = ProjectTask.ProjectTaskStatus.ToDo, Priority = ProjectTask.ProjectTaskPriority.Low, UserId = 1, ProjectId = 1 },
            new ProjectTaskDTO { ProjectTaskId = 2, Title = "Task 2", Description = "Description 2", DueDate = DateTime.Now.AddDays(2), Status = ProjectTask.ProjectTaskStatus.InProgress, Priority = ProjectTask.ProjectTaskPriority.High, UserId = 2, ProjectId = 2 }
        };

        // Act
        var tasks = taskDTOs.ToProjectTaskList().ToList();

        // Assert
        Assert.NotNull(tasks);
        Assert.AreEqual(taskDTOs.Count, tasks.Count);
        Assert.AreEqual(taskDTOs.First().ProjectTaskId, tasks.First().ProjectTaskId);
        Assert.AreEqual(taskDTOs.Last().ProjectTaskId, tasks.Last().ProjectTaskId);
    }

    [Test]
    public void ToProjectTask_ShouldReturnNull_WhenDTOIsNull()
    {
        // Act
        var task = (null as ProjectTaskDTO).ToProjectTask();

        // Assert
        Assert.IsNull(task);
    }

    [Test]
    public void ToProjectTaskDTO_ShouldReturnNull_WhenTaskIsNull()
    {
        // Act
        var taskDTO = (null as ProjectTask).ToProjectTaskDTO();

        // Assert
        Assert.IsNull(taskDTO);
    }

    [Test]
    public void ToProjectTaskDTOFull_ShouldReturnNull_WhenTaskIsNull()
    {
        // Act
        var taskDTOFull = (null as ProjectTask).ToProjectTaskDTOFull();

        // Assert
        Assert.IsNull(taskDTOFull);
    }
}