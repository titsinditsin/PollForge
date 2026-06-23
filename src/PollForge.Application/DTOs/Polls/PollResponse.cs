using PollForge.Domain.Enums;

namespace PollForge.Application.DTOs.Polls;

public record PollResponse(
    Guid Id,
    string Title,
    string? Description,
    Guid AuthorId,
    PollType Type,
    PollStatus Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset? ClosesAt,
    List<PollOptionResponse> Options);
