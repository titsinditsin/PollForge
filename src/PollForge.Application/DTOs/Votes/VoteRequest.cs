namespace PollForge.Application.DTOs.Votes;

public record VoteRequest(Guid OptionId, string? Fingerprint);
