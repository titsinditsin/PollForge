using PollForge.Domain.Entities;

namespace PollForge.Domain.Interfaces;

public interface IPollRepository
{
    Task<Poll?> GetByIdAsync(Guid id);
    Task<(List<Poll> Items, int TotalCount)> GetPagedAsync(int page, int pageSize);
    Task AddAsync(Poll poll);
    void Update(Poll poll);
    void Delete(Poll poll);
}
