using Microsoft.AspNetCore.Mvc;
using SisandUserManagement.API.DTOs;
using SisandUserManagement.Application.Interfaces.Services;

namespace SisandUserManagement.API.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        var user = await _authService.RegisterAsync(request.Name, request.Email, request.Password, request.Role);
        return Ok(new { user.Id, user.Name, user.Email });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var token = await _authService.AuthenticateAsync(request.Email, request.Password);
        if (token == null)
            return Unauthorized();

        return Ok(new { Token = token });
    }
}
