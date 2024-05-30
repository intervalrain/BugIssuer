using BugIssuer.Domain;
using BugIssuer.Domain.Enums;
using BugIssuer.Test.Common;

using ErrorOr;

using FluentAssertions;

namespace BugIssuer.Test.DomainUnitTests;

[TestClass]
public class IssueTests
{
    [TestMethod]
    public void CreateIssue_WhenConstructedSuccessfully_ShouldBeOpenAndNoLabel()
    {
        // Arrange
        var info = IssueFactory.CreateInfo();
        var user = CurrentUserFactory.Create();

        // Act
        var issue = Issue.Create(info.Title, info.Category, user.UserId, user.UserName, info.Description, info.Urgency, info.DateTime);

        // Assert
        issue.Status.Should().Be(Status.Open);
        issue.Label.Should().Be(Label.None);
        issue.Assignee.Should().Be(string.Empty);
    }

    [TestMethod]
    public void DeleteIssue_WhenIssueNotDeletedOrClosed_ShouldHaveDeleted()
    {
        // Arragne
        var issue = IssueFactory.CreateIssue();

        // Act
        var result = issue.Delete();

        // Assert
        result.IsError.Should().BeFalse();
        issue.Status.Should().Be(Status.Deleted);
    }

    [TestMethod]
    public void DeleteIssue_WhenIssueClosed_ShouldReturnUnauthorized()
    {
        // Arragne
        var issue = IssueFactory.CreateIssue();

        // Act
        var result1 = issue.Close();
        var result2 = issue.Delete();

        // Assert
        result1.IsError.Should().BeFalse();

        result2.IsError.Should().BeTrue();
        result2.FirstError.Type.Should().Be(ErrorType.Unauthorized);

        issue.Status.Should().Be(Status.Closed);
    }

    [TestMethod]
    public void DeleteIssue_WhenIssueDeleted_ShouldReturnNotFound()
    {
        // Arragne
        var issue = IssueFactory.CreateIssue();

        // Act
        var result1 = issue.Delete();
        var result2 = issue.Delete();

        // Assert
        result1.IsError.Should().BeFalse();

        result2.IsError.Should().BeTrue();
        result2.FirstError.Type.Should().Be(ErrorType.NotFound);

        issue.Status.Should().Be(Status.Deleted);
    }

    [TestMethod]
    public void EditIssue_WhenIssueNotDeletedOrClosed_ShouldHaveEdited()
    {
        // Arrange
        var issue = IssueFactory.CreateIssue();
        var newInfo = IssueFactory.CreateInfo();

        // Act
        var result = issue.Update(newInfo.Title, newInfo.Description, newInfo.Category, newInfo.Urgency, newInfo.DateTime);

        // Assert
        result.IsError.Should().BeFalse();
        issue.Title.Should().Be(newInfo.Title);
        issue.Description.Should().Be(newInfo.Description);
        issue.Category.Should().Be(newInfo.Category);
        issue.Urgency.Should().Be(newInfo.Urgency);
        issue.LastUpdate.Should().Be(newInfo.DateTime);
    }

    [TestMethod]
    public void EditIssue_WhenIssueDeleted_ShouldReturnNotFound()
    {
        // Arrange
        var issue = IssueFactory.CreateIssue();
        var newInfo = IssueFactory.CreateInfo();

        // Act
        var result1 = issue.Delete();
        var result2 = issue.Update(newInfo.Title, newInfo.Description, newInfo.Category, newInfo.Urgency, newInfo.DateTime);

        // Assert
        result1.IsError.Should().BeFalse();

        result2.IsError.Should().BeTrue();
        result2.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [TestMethod]
    public void EditIssue_WhenIssueClosed_ShouldReturnUnauthorized()
    {
        // Arrange
        var issue = IssueFactory.CreateIssue();
        var newInfo = IssueFactory.CreateInfo();

        // Act
        var result1 = issue.Close();
        var result2 = issue.Update(newInfo.Title, newInfo.Description, newInfo.Category, newInfo.Urgency, newInfo.DateTime);

        // Assert
        result1.IsError.Should().BeFalse();

        result2.IsError.Should().BeTrue();
        result2.FirstError.Type.Should().Be(ErrorType.Unauthorized);
    }

    [TestMethod]
    public void CloseIssue_WhenIssueOpenOrOngoing_ShouldBeClosed()
    {
        // Arrange
        var issue = IssueFactory.CreateIssue();

        // Act
        var result = issue.Close();

        // Assert
        result.IsError.Should().BeFalse();
        issue.Status.Should().Be(Status.Closed);
    }

    [TestMethod]
    public void CloseIssue_WhenIssueAlreadyClosed_ShouldReturnConflict()
    {
        // Arrange
        var issue = IssueFactory.CreateIssue();

        // Act
        var result1 = issue.Close();
        var result2 = issue.Close();

        // Assert
        result1.IsError.Should().BeFalse();

        result2.IsError.Should().BeTrue();
        result2.FirstError.Type.Should().Be(ErrorType.Conflict);
    }

    [TestMethod]
    public void CloseIssue_WhenIssueDeleted_ShouldReturnNotFound()
    {
        // Arrange
        var issue = IssueFactory.CreateIssue();
        
        // Act
        var result1 = issue.Delete();
        var result2 = issue.Close();

        // Assert
        result1.IsError.Should().BeFalse();

        result2.IsError.Should().BeTrue();
        result2.FirstError.Type.Should().Be(ErrorType.NotFound);

        issue.Status.Should().Be(Status.Deleted);
    }

    [TestMethod]
    public void AssignIssue_WhenIssueNotClosedOrDeleted_ShouldHaveAssigned()
    {
        // Arrange
        var issue = IssueFactory.CreateIssue();
        var assignee = StringGenerator.GenerateUserName();

        // Act
        var result = issue.Assign(assignee);

        // Assert
        result.IsError.Should().BeFalse();
        issue.Assignee.Should().Be(assignee);
        issue.Status.Should().Be(Status.Ongoing);
    }

    [TestMethod]
    public void AssignIssue_WhenIssueClosed_ShouldHaveAssigned()
    {
        // Arrange
        var issue = IssueFactory.CreateIssue();
        var assignee = StringGenerator.GenerateUserName();

        // Act
        var result1 = issue.Close();
        var result2 = issue.Assign(assignee);

        // Assert
        result1.IsError.Should().BeFalse();

        result2.IsError.Should().BeFalse();
        issue.Status.Should().Be(Status.Ongoing);
        issue.Assignee.Should().Be(assignee);
    }

    [TestMethod]
    public void AssignIssue_WhenIssueDeleted_ShouldReturnNotFound()
    {
        // Arrange
        var issue = IssueFactory.CreateIssue();
        var assignee = StringGenerator.GenerateUserName();

        // Act
        var result1 = issue.Delete();
        var result2 = issue.Assign(assignee);

        // Assert
        result1.IsError.Should().BeFalse();

        result2.IsError.Should().BeTrue();
        result2.FirstError.Type.Should().Be(ErrorType.NotFound);
        issue.Status.Should().Be(Status.Deleted);
    }

    [TestMethod]
    public void UnassignIssue_WhenIssueOngoing_ShouldHaveUnassigned()
    {
        // Arrange
        var issue = IssueFactory.CreateIssue();
        var assignee = StringGenerator.GenerateUserName();

        // Act
        var result1 = issue.Assign(assignee);
        var result2 = issue.Unassign();

        // Assert
        result1.IsError.Should().BeFalse();

        result2.IsError.Should().BeFalse();
        issue.Assignee.Should().Be(string.Empty);
        issue.Status.Should().Be(Status.Open);
    }

    [TestMethod]
    public void ReopenIssue_WhenIssueDeleted_ShouldHaveReopened()
    {
        // Arrange
        var issue = IssueFactory.CreateIssue();

        // Act
        var result1 = issue.Delete();
        var result2 = issue.Reopen();

        // Assert
        result1.IsError.Should().BeFalse();

        result2.IsError.Should().BeFalse();
        issue.Status.Should().Be(Status.Open);
    }

    [TestMethod]
    public void ReopenIssue_WhenIssueClosed_ShouldHaveReopened()
    {
        // Arrange
        var issue = IssueFactory.CreateIssue();
        var assignee = StringGenerator.GenerateUserName();

        // Act
        var result1 = issue.Assign(assignee);
        var result2 = issue.Close();
        var result3 = issue.Reopen();

        // Assert
        result1.IsError.Should().BeFalse();

        result2.IsError.Should().BeFalse();

        result3.IsError.Should().BeFalse();
        issue.Status.Should().Be(Status.Ongoing);
    }

    [TestMethod]
    public void AddComment_WhenIssueNotClosedOrDeleted_ShouldHaveCommented()
    {
        // Arrange
        var issue = IssueFactory.CreateIssue();
        var currentUser = CurrentUserFactory.Create();
        var content = StringGenerator.GenerateContent();

        // Act
        var result = issue.AddComment(currentUser.UserId, currentUser.UserName, content, DateTime.Now);

        // Assert
        result.IsError.Should().BeFalse();
        issue.Comments.Count().Should().Be(1);
    }


    [TestMethod]
    public void AddComment_WhenIssueDeleted_ShouldHaveCommented()
    {
        // Arrange
        var issue = IssueFactory.CreateIssue();
        var currentUser = CurrentUserFactory.Create();
        var content = StringGenerator.GenerateContent();

        // Act
        var result1 = issue.Delete();
        var result2 = issue.AddComment(currentUser.UserId, currentUser.UserName, content, DateTime.Now);

        // Assert
        result1.IsError.Should().BeFalse();

        result2.IsError.Should().BeTrue();
        result2.FirstError.Type.Should().Be(ErrorType.NotFound);

        issue.Comments.Count().Should().Be(0);
    }
}

