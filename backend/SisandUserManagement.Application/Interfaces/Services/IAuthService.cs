using SisandUserManagement.Domain.Entities;

namespace SisandUserManagement.Application.Interfaces.Services;

public interface IAuthService
{
    Task<string?> AuthenticateAsync(string username, string password);
    Task<User> RegisterAsync(User user);
}
