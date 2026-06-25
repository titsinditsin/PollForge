using Microsoft.AspNetCore.Mvc;
using PollForge.Application.DTOs.Auth;
using PollForge.Application.Interfaces;
using PollForge.Domain.Entities;
using PollForge.Domain.Interfaces;

namespace PollForge.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;

    public AuthController(IUserRepository userRepository, IAuthService authService, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _authService = authService;
        _unitOfWork = unitOfWork;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        if (await _userRepository.ExistsByEmailAsync(request.Email))
        {
            return BadRequest(new ProblemDetails { Title = "Registration Failed", Detail = "Email is already taken." });
        }

        var user = PollForge.Domain.Entities.User.Create(request.Username, request.Email, _authService.HashPassword(request.Password));
        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        var token = _authService.GenerateToken(user);
        return Ok(new AuthResponse(user.Id, token, DateTimeOffset.UtcNow.AddMinutes(60)));
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null || !_authService.VerifyPassword(request.Password, user.PasswordHash))
        {
            return Unauthorized(new ProblemDetails { Title = "Login Failed", Detail = "Invalid email or password." });
        }

        var token = _authService.GenerateToken(user);
        return Ok(new AuthResponse(user.Id, token, DateTimeOffset.UtcNow.AddMinutes(60)));
    }
}
