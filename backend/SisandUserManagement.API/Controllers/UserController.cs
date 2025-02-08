using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SisandUserManagement.API.DTOs;
using SisandUserManagement.Application.Interfaces.Services;
using SisandUserManagement.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace SisandUserManagement.API.Controllers;

[Route("api/users")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UserController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
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
    [SwaggerOperation(Summary = "Lista usuários com paginação e campos personalizados.")]
    public async Task<IActionResult> GetAllUsers(
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10, 
        [FromQuery] string? fields = null, 
        [FromQuery] string? filter = null)
    {
        if (page < 1 || pageSize < 1)
            return BadRequest("Página e tamanho da página devem ser maiores que 0.");

        var selectedFields = fields?.Split(',').Select(f => f.Trim()).ToList();
        var (users, totalUsers) = await _userService.GetAllUsersPaginatedAsync(page, pageSize, selectedFields, filter);

        var totalPages = (int)Math.Ceiling((double)totalUsers / pageSize);

        return Ok(new
        {
            TotalUsers = totalUsers,
            TotalPages = totalPages,
            CurrentPage = page,
            PageSize = pageSize,
            Users = users
        });
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
        var existingUser = await _userService.GetUserByIdAsync(id);
        if (existingUser == null) return NotFound("Usuário não encontrado.");

        var updatedUser = _mapper.Map<User>(request);

        var success = await _userService.UpdateUserAsync(id, updatedUser);

        if (!success)
            return BadRequest("Falha ao atualizar o usuário.");

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