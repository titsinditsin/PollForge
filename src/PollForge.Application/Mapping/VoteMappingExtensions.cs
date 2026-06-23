using PollForge.Application.DTOs.Votes;
using PollForge.Domain.Entities;

namespace PollForge.Application.Mapping;

public static class VoteMappingExtensions
{
    public static VoteResponse ToResponse(this Vote vote)
    {
        return new VoteResponse(
            vote.Id,
            vote.PollId,
            vote.OptionId,
            vote.CreatedAt);
    }
}
