using PollForge.Application.DTOs.Polls;
using PollForge.Domain.Entities;

namespace PollForge.Application.Mapping;

public static class PollMappingExtensions
{
    public static PollResponse ToResponse(this Poll poll)
    {
        return new PollResponse(
            poll.Id,
            poll.Title,
            poll.Description,
            poll.AuthorId,
            poll.Type,
            poll.Status,
            poll.CreatedAt,
            poll.ClosesAt,
            poll.Options.Select(o => o.ToResponse()).ToList());
    }

    public static PollOptionResponse ToResponse(this PollOption option)
    {
        return new PollOptionResponse(
            option.Id,
            option.Text,
            option.DisplayOrder);
    }
}
