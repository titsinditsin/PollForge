namespace PollForge.Application.DTOs.Votes;

public record OptionResultResponse(Guid OptionId, string Text, int VoteCount, double Percentage);
