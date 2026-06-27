using PollForge.Domain.Entities;
using Xunit;

namespace PollForge.UnitTests.Domain;

public class VoteTests
{
    [Fact]
    public void Create_ShouldInitializeVoteCorrectly()
    {
        var pollId = Guid.NewGuid();
        var optionId = Guid.NewGuid();
        var voterId = Guid.NewGuid();
        var fingerprint = "fp-123456";
        var ipAddress = "192.168.1.1";

        var vote = Vote.Create(pollId, optionId, voterId, fingerprint, ipAddress);

        Assert.NotEqual(Guid.Empty, vote.Id);
        Assert.Equal(pollId, vote.PollId);
        Assert.Equal(optionId, vote.OptionId);
        Assert.Equal(voterId, vote.VoterId);
        Assert.Equal(fingerprint, vote.Fingerprint);
        Assert.Equal(ipAddress, vote.IpAddress);
        Assert.True((DateTimeOffset.UtcNow - vote.CreatedAt).TotalSeconds < 2);
    }
}
