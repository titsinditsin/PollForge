namespace PollForge.Application.DTOs.Votes;

public record PollResultsResponse(
    Guid PollId,
    string Title,
    int TotalVotes,
    List<OptionResultResponse> Options);
