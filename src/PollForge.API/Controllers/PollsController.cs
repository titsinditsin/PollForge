using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PollForge.Application.DTOs.Polls;
using PollForge.Application.Interfaces;
using PollForge.Application.Mapping;
using PollForge.Domain.Entities;
using PollForge.Domain.Interfaces;

namespace PollForge.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PollsController : ControllerBase
{
    private readonly IPollRepository _pollRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public PollsController(IPollRepository pollRepository, ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
    {
        _pollRepository = pollRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<PollResponse>>> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var (items, totalCount) = await _pollRepository.GetPagedAsync(page, pageSize);
        var response = new PagedResponse<PollResponse>(
            items.Select(p => p.ToResponse()).ToList(),
            totalCount,
            page,
            pageSize);

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PollResponse>> GetById(Guid id)
    {
        var poll = await _pollRepository.GetByIdAsync(id);
        if (poll == null) return NotFound();

        return Ok(poll.ToResponse());
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<PollResponse>> Create(CreatePollRequest request)
    {
        var userId = _currentUserService.UserId;
        if (userId == null) return Unauthorized();

        var poll = Poll.Create(request.Title, request.Description, userId.Value, request.Type, request.ClosesAt);
        foreach (var optionText in request.Options)
        {
            poll.AddOption(optionText);
        }

        await _pollRepository.AddAsync(poll);
        await _unitOfWork.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = poll.Id }, poll.ToResponse());
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update(Guid id, UpdatePollRequest request)
    {
        var poll = await _pollRepository.GetByIdAsync(id);
        if (poll == null) return NotFound();

        if (poll.AuthorId != _currentUserService.UserId) return Forbid();

        _pollRepository.Update(poll);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var poll = await _pollRepository.GetByIdAsync(id);
        if (poll == null) return NotFound();

        if (poll.AuthorId != _currentUserService.UserId) return Forbid();

        _pollRepository.Delete(poll);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }
}
