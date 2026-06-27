using Microsoft.AspNetCore.Mvc;
using PollForge.Application.DTOs.Votes;
using PollForge.Application.Interfaces;
using PollForge.Application.Mapping;
using PollForge.Domain.Entities;
using PollForge.Domain.Enums;
using PollForge.Domain.Interfaces;

namespace PollForge.API.Controllers;

[ApiController]
[Route("api/polls/{pollId:guid}")]
public class VotesController : ControllerBase
{
    private readonly IPollRepository _pollRepository;
    private readonly IVoteRepository _voteRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public VotesController(
        IPollRepository pollRepository,
        IVoteRepository voteRepository,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        _pollRepository = pollRepository;
        _voteRepository = voteRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    [HttpPost("votes")]
    public async Task<ActionResult<VoteResponse>> CastVote(Guid pollId, VoteRequest request)
    {
        var poll = await _pollRepository.GetByIdAsync(pollId);
        if (poll == null) return NotFound(new ProblemDetails { Title = "Not Found", Detail = "Poll not found." });

        if (poll.Status == PollStatus.Closed || (poll.ClosesAt.HasValue && poll.ClosesAt.Value < DateTimeOffset.UtcNow))
        {
            return BadRequest(new ProblemDetails { Title = "Poll Closed", Detail = "This poll is no longer accepting votes." });
        }

        if (!poll.Options.Any(o => o.Id == request.OptionId))
        {
            return BadRequest(new ProblemDetails { Title = "Invalid Option", Detail = "The selected option does not belong to this poll." });
        }

        // Anti-fraud: determine fingerprint (use client IP if fingerprint is missing)
        var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var fingerprint = !string.IsNullOrWhiteSpace(request.Fingerprint) ? request.Fingerprint : clientIp;

        // Check if vote already exists for this fingerprint
        if (await _voteRepository.ExistsByFingerprintAsync(pollId, fingerprint))
        {
            return Conflict(new ProblemDetails { Title = "Already Voted", Detail = "A vote has already been cast from this device/fingerprint for this poll." });
        }

        var vote = Vote.Create(pollId, request.OptionId, _currentUserService.UserId, fingerprint, clientIp);
        await _voteRepository.AddAsync(vote);
        await _unitOfWork.SaveChangesAsync();

        return CreatedAtAction(nameof(GetResults), new { pollId }, vote.ToResponse());
    }

    [HttpGet("results")]
    public async Task<ActionResult<PollResultsResponse>> GetResults(Guid pollId)
    {
        var poll = await _pollRepository.GetByIdAsync(pollId);
        if (poll == null) return NotFound();

        var votes = await _voteRepository.GetByPollIdAsync(pollId);
        var totalVotes = votes.Count;

        var optionResults = poll.Options.Select(o =>
        {
            var count = votes.Count(v => v.OptionId == o.Id);
            var percentage = totalVotes > 0 ? Math.Round((double)count / totalVotes * 100, 2) : 0;
            return new OptionResultResponse(o.Id, o.Text, count, percentage);
        }).ToList();

        return Ok(new PollResultsResponse(poll.Id, poll.Title, totalVotes, optionResults));
    }
}
