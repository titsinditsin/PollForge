using PollForge.Domain.Enums;

namespace PollForge.Application.DTOs.Polls;

public record CreatePollRequest(
    string Title,
    string? Description,
    PollType Type,
    DateTimeOffset? ClosesAt,
    List<string> Options);
