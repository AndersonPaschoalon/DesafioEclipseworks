namespace EclipseTaskManager.Unit.DTOsTests;

using EclipseTaskManager.DTOs;
using EclipseTaskManager.DTOs.Mappings;
using EclipseTaskManager.Models;


[TestFixture]
public class ProjectTaskCommentDTOMappingTests
{
    [Test]
    public void ToProjectTaskCommentDTO_ShouldConvertProjectTaskCommentToDTO()
    {
        // Arrange
        var comment = new ProjectTaskComment
        {
            ProjectTaskCommentId = 1,
            Comment = "This is a comment",
            UserId = 100,
            ProjectId = 200,
            ProjectTaskId = 300
        };

        // Act
        var commentDTO = comment.ToProjectTaskCommentDTO();

        // Assert
        Assert.NotNull(commentDTO);
        Assert.AreEqual(comment.ProjectTaskCommentId, commentDTO.ProjectTaskCommentId);
        Assert.AreEqual(comment.Comment, commentDTO.Comment);
        Assert.AreEqual(comment.UserId, commentDTO.UserId);
        Assert.AreEqual(comment.ProjectId, commentDTO.ProjectId);
        Assert.AreEqual(comment.ProjectTaskId, commentDTO.ProjectTaskId);
    }

    [Test]
    public void ToProjectTaskComment_ShouldConvertDTOToProjectTaskComment()
    {
        // Arrange
        var commentDTO = new ProjectTaskCommentDTO
        {
            ProjectTaskCommentId = 1,
            Comment = "This is a comment",
            UserId = 100,
            ProjectId = 200,
            ProjectTaskId = 300
        };

        // Act
        var comment = commentDTO.ToProjectTaskComment();

        // Assert
        Assert.NotNull(comment);
        Assert.AreEqual(commentDTO.ProjectTaskCommentId, comment.ProjectTaskCommentId);
        Assert.AreEqual(commentDTO.Comment, comment.Comment);
        Assert.AreEqual(commentDTO.UserId, comment.UserId);
        Assert.AreEqual(commentDTO.ProjectId, comment.ProjectId);
        Assert.AreEqual(commentDTO.ProjectTaskId, comment.ProjectTaskId);
    }

    [Test]
    public void ToProjectTaskCommentDTOList_ShouldConvertListOfCommentsToListOfDTOs()
    {
        // Arrange
        var comments = new List<ProjectTaskComment>
        {
            new ProjectTaskComment { ProjectTaskCommentId = 1, Comment = "Comment 1", UserId = 100, ProjectId = 200, ProjectTaskId = 300 },
            new ProjectTaskComment { ProjectTaskCommentId = 2, Comment = "Comment 2", UserId = 101, ProjectId = 201, ProjectTaskId = 301 }
        };

        // Act
        var commentDTOs = comments.ToProjectTaskCommentDTOList().ToList();

        // Assert
        Assert.NotNull(commentDTOs);
        Assert.AreEqual(comments.Count, commentDTOs.Count);
        Assert.AreEqual(comments.First().ProjectTaskCommentId, commentDTOs.First().ProjectTaskCommentId);
        Assert.AreEqual(comments.Last().ProjectTaskCommentId, commentDTOs.Last().ProjectTaskCommentId);
    }

    [Test]
    public void ToProjectTaskCommentDTOList_EmptyList_ShouldReturnEmptyList()
    {
        // Arrange
        var comments = new List<ProjectTaskComment>();

        // Act
        var commentDTOs = comments.ToProjectTaskCommentDTOList();

        // Assert
        Assert.NotNull(commentDTOs);
        Assert.IsEmpty(commentDTOs);
    }

    [Test]
    public void ToProjectTaskComment_ShouldReturnNull_WhenDTOIsNull()
    {
        // Act
        var comment = (null as ProjectTaskCommentDTO).ToProjectTaskComment();

        // Assert
        Assert.IsNull(comment);
    }

    [Test]
    public void ToProjectTaskCommentDTO_ShouldReturnNull_WhenCommentIsNull()
    {
        // Act
        var commentDTO = (null as ProjectTaskComment).ToProjectTaskCommentDTO();

        // Assert
        Assert.IsNull(commentDTO);
    }
}