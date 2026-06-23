using PollForge.Domain.Entities;

namespace PollForge.Domain.Interfaces;

public interface IVoteRepository
{
    Task AddAsync(Vote vote);
    Task<List<Vote>> GetByPollIdAsync(Guid pollId);
    Task<bool> ExistsByFingerprintAsync(Guid pollId, string fingerprint);
    Task<int> CountByOptionIdAsync(Guid optionId);
}
