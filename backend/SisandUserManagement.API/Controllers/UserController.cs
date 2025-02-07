using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SisandUserManagement.API.DTOs;
using SisandUserManagement.Application.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace SisandUserManagement.API.Controllers;

[Route("api/users")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Obtém os dados do usuário autenticado.
    /// </summary>
    /// <returns>Retorna informações do usuário logado.</returns>
    [HttpGet("me")]
    [SwaggerOperation(Summary = "Obtém os dados do usuário autenticado.")]
    public async Task<IActionResult> GetUserProfile()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized("Usuário não autenticado.");

        var user = await _userService.GetAuthenticatedUserAsync(userId);
        if (user == null)
            return NotFound("Usuário não encontrado.");

        return Ok(new
        {
            UserId = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role
        });
    }

    /// <summary>
    /// Lista todos os usuários do sistema.
    /// </summary>
    /// <returns>Retorna uma lista com todos os usuários.</returns>
    [HttpGet]
    [SwaggerOperation(Summary = "Lista todos os usuários do sistema.")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    /// <summary>
    /// Obtém um usuário pelo ID.
    /// </summary>
    /// <param name="id">ID do usuário</param>
    /// <returns>Retorna os detalhes do usuário.</returns>
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Obtém um usuário pelo ID.")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
            return NotFound("Usuário não encontrado.");

        return Ok(user);
    }

    /// <summary>
    /// Atualiza os dados de um usuário.
    /// </summary>
    /// <param name="id">ID do usuário</param>
    /// <param name="request">Dados do usuário a serem atualizados</param>
    /// <returns>NoContent se atualizado com sucesso.</returns>
    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Atualiza os dados de um usuário.")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequestDto request)
    {
        var success = await _userService.UpdateUserAsync(id, request.Name, request.Email);
        if (!success)
            return NotFound("Usuário não encontrado.");

        return NoContent();
    }

    /// <summary>
    /// Deleta um usuário do sistema.
    /// </summary>
    /// <param name="id">ID do usuário</param>
    /// <returns>NoContent se deletado com sucesso.</returns>
    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Deleta um usuário do sistema.")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var success = await _userService.DeleteUserAsync(id);
        if (!success)
            return NotFound("Usuário não encontrado.");

        return NoContent();
    }
}