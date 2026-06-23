namespace PollForge.Application.DTOs.Votes;

public record VoteResponse(Guid Id, Guid PollId, Guid OptionId, DateTimeOffset CreatedAt);
