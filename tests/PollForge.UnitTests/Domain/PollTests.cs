using PollForge.Domain.Entities;
using PollForge.Domain.Enums;
using Xunit;

namespace PollForge.UnitTests.Domain;

public class PollTests
{
    [Fact]
    public void Create_ShouldInitializeDraftPollWithEmptyOptions()
    {
        var authorId = Guid.NewGuid();
        var poll = Poll.Create("Favorite Language", "Select one", authorId, PollType.SingleChoice, null);

        Assert.NotEqual(Guid.Empty, poll.Id);
        Assert.Equal("Favorite Language", poll.Title);
        Assert.Equal("Select one", poll.Description);
        Assert.Equal(authorId, poll.AuthorId);
        Assert.Equal(PollStatus.Draft, poll.Status);
        Assert.Empty(poll.Options);
    }

    [Fact]
    public void AddOption_ShouldIncrementDisplayOrder()
    {
        var poll = Poll.Create("Test Poll", null, Guid.NewGuid(), PollType.SingleChoice, null);

        poll.AddOption("Option 1");
        poll.AddOption("Option 2");

        Assert.Equal(2, poll.Options.Count);
        Assert.Equal("Option 1", poll.Options[0].Text);
        Assert.Equal(1, poll.Options[0].DisplayOrder);
        Assert.Equal("Option 2", poll.Options[1].Text);
        Assert.Equal(2, poll.Options[1].DisplayOrder);
    }

    [Fact]
    public void Activate_WithLessThanTwoOptions_ShouldThrowException()
    {
        var poll = Poll.Create("Test Poll", null, Guid.NewGuid(), PollType.SingleChoice, null);
        poll.AddOption("Only One");

        Assert.Throws<InvalidOperationException>(() => poll.Activate());
    }

    [Fact]
    public void Activate_WithTwoOrMoreOptions_ShouldSetStatusToActive()
    {
        var poll = Poll.Create("Test Poll", null, Guid.NewGuid(), PollType.SingleChoice, null);
        poll.AddOption("Option 1");
        poll.AddOption("Option 2");

        poll.Activate();

        Assert.Equal(PollStatus.Active, poll.Status);
    }

    [Fact]
    public void Close_ActivePoll_ShouldSetStatusToClosed()
    {
        var poll = Poll.Create("Test Poll", null, Guid.NewGuid(), PollType.SingleChoice, null);
        poll.AddOption("Option 1");
        poll.AddOption("Option 2");
        poll.Activate();

        poll.Close();

        Assert.Equal(PollStatus.Closed, poll.Status);
    }
}
