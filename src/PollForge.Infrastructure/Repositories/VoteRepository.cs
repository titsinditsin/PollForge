using Microsoft.EntityFrameworkCore;
using PollForge.Domain.Entities;
using PollForge.Domain.Interfaces;
using PollForge.Infrastructure.Data;

namespace PollForge.Infrastructure.Repositories;

public class VoteRepository(AppDbContext context) : IVoteRepository
{
    public async Task AddAsync(Vote vote)
    {
        await context.Votes.AddAsync(vote);
    }

    public async Task<List<Vote>> GetByPollIdAsync(Guid pollId)
    {
        return await context.Votes
            .Where(v => v.PollId == pollId)
            .ToListAsync();
    }

    public async Task<bool> ExistsByFingerprintAsync(Guid pollId, string fingerprint)
    {
        return await context.Votes
            .AnyAsync(v => v.PollId == pollId && v.Fingerprint == fingerprint);
    }

    public async Task<int> CountByOptionIdAsync(Guid optionId)
    {
        return await context.Votes
            .CountAsync(v => v.OptionId == optionId);
    }
}
