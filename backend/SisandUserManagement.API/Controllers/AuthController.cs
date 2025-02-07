using Microsoft.AspNetCore.Mvc;
using SisandUserManagement.API.DTOs;
using SisandUserManagement.Application.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace SisandUserManagement.API.Controllers;

/// <summary>
/// Controlador responsável pela autenticação e registro de usuários.
/// </summary>
[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Registra um novo usuário no sistema.
    /// </summary>
    /// <param name="request">Dados do novo usuário.</param>
    /// <returns>Retorna os dados do usuário cadastrado.</returns>
    /// <response code="200">Usuário cadastrado com sucesso.</response>
    /// <response code="400">Erro ao cadastrar o usuário.</response>
    [HttpPost("register")]
    [SwaggerOperation(Summary = "Registra um novo usuário.", Description = "Cria um novo usuário no sistema.")]
    [SwaggerResponse(200, "Usuário cadastrado com sucesso.", typeof(RegisterRequestDto))]
    [SwaggerResponse(400, "Erro ao cadastrar o usuário.")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                                          .Select(e => e.ErrorMessage)
                                          .ToList();
            return BadRequest(new { message = "Erro de validação.", errors });
        }

        try
        {
            var user = await _authService.RegisterAsync(request.Name, request.Email, request.Password, request.Role);
            return Ok(new { user.Id, user.Name, user.Email, user.Role });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Autentica um usuário e retorna um token JWT.
    /// </summary>
    /// <param name="request">Credenciais do usuário.</param>
    /// <returns>Token JWT.</returns>
    /// <response code="200">Autenticação bem-sucedida.</response>
    /// <response code="401">Credenciais inválidas.</response>
    [HttpPost("login")]
    [SwaggerOperation(Summary = "Autentica um usuário.", Description = "Autentica um usuário e retorna um token JWT.")]
    [SwaggerResponse(200, "Autenticação bem-sucedida.", typeof(string))]
    [SwaggerResponse(401, "Credenciais inválidas.")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var token = await _authService.AuthenticateAsync(request.Email, request.Password);
        if (token == null)
            return Unauthorized();

        return Ok(new { Token = token });
    }
}
