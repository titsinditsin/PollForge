using Microsoft.EntityFrameworkCore;
using PollForge.Domain.Entities;
using PollForge.Domain.Interfaces;
using PollForge.Infrastructure.Data;

namespace PollForge.Infrastructure.Repositories;

public class PollRepository(AppDbContext context) : IPollRepository
{
    public async Task<Poll?> GetByIdAsync(Guid id)
    {
        return await context.Polls
            .Include(p => p.Options)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<(List<Poll> Items, int TotalCount)> GetPagedAsync(int page, int pageSize)
    {
        var totalCount = await context.Polls.CountAsync();

        var items = await context.Polls
            .Include(p => p.Options)
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task AddAsync(Poll poll)
    {
        await context.Polls.AddAsync(poll);
    }

    public void Update(Poll poll)
    {
        context.Polls.Update(poll);
    }

    public void Delete(Poll poll)
    {
        context.Polls.Remove(poll);
    }
}
