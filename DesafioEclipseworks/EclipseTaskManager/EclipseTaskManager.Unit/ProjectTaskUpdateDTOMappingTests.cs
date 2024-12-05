namespace EclipseTaskManager.Unit.DTOsTests;

using EclipseTaskManager.DTOs;
using EclipseTaskManager.DTOs.Mappings;
using EclipseTaskManager.Models;


[TestFixture]
public class ProjectTaskUpdateDTOMappingTests
{
    [Test]
    public void ToProjectTaskUpdateDTO_ShouldConvertProjectTaskUpdateToDTO()
    {
        // Arrange
        var update = new ProjectTaskUpdate
        {
            ProjectTaskUpdateId = 1,
            ModifiedField = "Field1",
            OldFieldValue = "OldValue1",
            NewFieldValue = "NewValue1",
            ModificationDate = DateTime.Now,
            UserId = 100,
            ProjectId = 200,
            ProjectTaskId = 300
        };

        // Act
        var updateDTO = update.ToProjectTaskUpdateDTO();

        // Assert
        Assert.NotNull(updateDTO);
        Assert.AreEqual(update.ProjectTaskUpdateId, updateDTO.ProjectTaskUpdateId);
        Assert.AreEqual(update.ModifiedField, updateDTO.ModifiedField);
        Assert.AreEqual(update.OldFieldValue, updateDTO.OldFieldValue);
        Assert.AreEqual(update.NewFieldValue, updateDTO.NewFieldValue);
        Assert.AreEqual(update.ModificationDate, updateDTO.ModificationDate);
        Assert.AreEqual(update.UserId, updateDTO.UserId);
        Assert.AreEqual(update.ProjectId, updateDTO.ProjectId);
        Assert.AreEqual(update.ProjectTaskId, updateDTO.ProjectTaskId);
    }

    [Test]
    public void ToProjectTaskUpdate_ShouldConvertDTOToProjectTaskUpdate()
    {
        // Arrange
        var updateDTO = new ProjectTaskUpdateDTO
        {
            ProjectTaskUpdateId = 1,
            ModifiedField = "Field1",
            OldFieldValue = "OldValue1",
            NewFieldValue = "NewValue1",
            ModificationDate = DateTime.Now,
            UserId = 100,
            ProjectId = 200,
            ProjectTaskId = 300
        };

        // Act
        var update = updateDTO.ToProjectTaskUpdate();

        // Assert
        Assert.NotNull(update);
        Assert.AreEqual(updateDTO.ProjectTaskUpdateId, update.ProjectTaskUpdateId);
        Assert.AreEqual(updateDTO.ModifiedField, update.ModifiedField);
        Assert.AreEqual(updateDTO.OldFieldValue, update.OldFieldValue);
        Assert.AreEqual(updateDTO.NewFieldValue, update.NewFieldValue);
        Assert.AreEqual(updateDTO.ModificationDate, update.ModificationDate);
        Assert.AreEqual(updateDTO.UserId, update.UserId);
        Assert.AreEqual(updateDTO.ProjectId, update.ProjectId);
        Assert.AreEqual(updateDTO.ProjectTaskId, update.ProjectTaskId);
    }

    [Test]
    public void ToProjectTaskUpdateDTOList_ShouldConvertListOfUpdatesToListOfDTOs()
    {
        // Arrange
        var updates = new List<ProjectTaskUpdate>
        {
            new ProjectTaskUpdate { ProjectTaskUpdateId = 1, ModifiedField = "Field1", OldFieldValue = "OldValue1", NewFieldValue = "NewValue1", ModificationDate = DateTime.Now, UserId = 100, ProjectId = 200, ProjectTaskId = 300 },
            new ProjectTaskUpdate { ProjectTaskUpdateId = 2, ModifiedField = "Field2", OldFieldValue = "OldValue2", NewFieldValue = "NewValue2", ModificationDate = DateTime.Now, UserId = 101, ProjectId = 201, ProjectTaskId = 301 }
        };

        // Act
        var updateDTOs = updates.ToProjectTaskUpdateDTOList().ToList();

        // Assert
        Assert.NotNull(updateDTOs);
        Assert.AreEqual(updates.Count, updateDTOs.Count);
        Assert.AreEqual(updates.First().ProjectTaskUpdateId, updateDTOs.First().ProjectTaskUpdateId);
        Assert.AreEqual(updates.Last().ProjectTaskUpdateId, updateDTOs.Last().ProjectTaskUpdateId);
    }

    [Test]
    public void ToProjectTaskUpdateDTOList_EmptyList_ShouldReturnEmptyList()
    {
        // Arrange
        var updates = new List<ProjectTaskUpdate>();

        // Act
        var updateDTOs = updates.ToProjectTaskUpdateDTOList().ToList();

        // Assert
        Assert.NotNull(updateDTOs);
        Assert.IsEmpty(updateDTOs);
    }

    [Test]
    public void ToProjectTaskUpdate_ShouldReturnNull_WhenDTOIsNull()
    {
        // Act
        var update = (null as ProjectTaskUpdateDTO).ToProjectTaskUpdate();

        // Assert
        Assert.IsNull(update);
    }

    [Test]
    public void ToProjectTaskUpdateDTO_ShouldReturnNull_WhenUpdateIsNull()
    {
        // Act
        var updateDTO = (null as ProjectTaskUpdate).ToProjectTaskUpdateDTO();

        // Assert
        Assert.IsNull(updateDTO);
    }
}