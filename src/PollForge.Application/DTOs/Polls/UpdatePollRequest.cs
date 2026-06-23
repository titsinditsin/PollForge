namespace PollForge.Application.DTOs.Polls;

public record UpdatePollRequest(string Title, string? Description, DateTimeOffset? ClosesAt);
