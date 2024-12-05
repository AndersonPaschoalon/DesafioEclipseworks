namespace EclipseTaskManager.Unit.DTOsTests;

using EclipseTaskManager.DTOs;
using EclipseTaskManager.Models;
using EclipseTaskManager.DTOs;
using EclipseTaskManager.DTOs.Mappings;


[TestFixture]
public class ProjectDTOMappingTests
{
    [Test]
    public void ToProjectDTO_ShouldConvertProjectToProjectDTO()
    {
        // Arrange
        var project = new Project
        {
            ProjectId = 1,
            Title = "Project 1",
            Description = "Description of Project 1",
            UserId = 10
        };

        // Act
        var projectDTO = project.ToProjectDTO();

        // Assert
        Assert.NotNull(projectDTO);
        Assert.AreEqual(project.ProjectId, projectDTO.ProjectId);
        Assert.AreEqual(project.Title, projectDTO.Title);
        Assert.AreEqual(project.Description, projectDTO.Description);
        Assert.AreEqual(project.UserId, projectDTO.UserId);
    }

    [Test]
    public void ToProject_ShouldConvertProjectDTOToProject()
    {
        // Arrange
        var projectDTO = new ProjectDTO
        {
            ProjectId = 1,
            Title = "Project 1",
            Description = "Description of Project 1",
            UserId = 10
        };

        // Act
        var project = projectDTO.ToProject();

        // Assert
        Assert.NotNull(project);
        Assert.AreEqual(projectDTO.ProjectId, project.ProjectId);
        Assert.AreEqual(projectDTO.Title, project.Title);
        Assert.AreEqual(projectDTO.Description, project.Description);
        Assert.AreEqual(projectDTO.UserId, project.UserId);
    }

    [Test]
    public void ToProjectDTOList_ShouldConvertListOfProjectsToListOfProjectDTOs()
    {
        // Arrange
        var projects = new List<Project>
        {
            new Project { ProjectId = 1, Title = "Project 1", Description = "Description 1", UserId = 10 },
            new Project { ProjectId = 2, Title = "Project 2", Description = "Description 2", UserId = 20 }
        };

        // Act
        var projectDTOs = projects.ToProjectDTOList().ToList();

        // Assert
        Assert.NotNull(projectDTOs);
        Assert.AreEqual(projects.Count, projectDTOs.Count);
        Assert.AreEqual(projects.First().ProjectId, projectDTOs.First().ProjectId);
        Assert.AreEqual(projects.Last().ProjectId, projectDTOs.Last().ProjectId);
    }

    [Test]
    public void ToProjectDTOList_EmptyList_ShouldReturnEmptyList()
    {
        // Arrange
        var projects = new List<Project>();

        // Act
        var projectDTOs = projects.ToProjectDTOList();

        // Assert
        Assert.NotNull(projectDTOs);
        Assert.IsEmpty(projectDTOs);
    }

    [Test]
    public void ToProject_ShouldReturnNull_WhenProjectDTOIsNull()
    {
        // Act
        var project = (null as ProjectDTO).ToProject();

        // Assert
        Assert.IsNull(project);
    }

    [Test]
    public void ToProjectDTO_ShouldReturnNull_WhenProjectIsNull()
    {
        // Act
        var projectDTO = (null as Project).ToProjectDTO();

        // Assert
        Assert.IsNull(projectDTO);
    }
}