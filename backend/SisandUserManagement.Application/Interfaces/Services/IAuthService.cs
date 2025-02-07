using SisandUserManagement.Domain.Entities;

namespace SisandUserManagement.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<string?> AuthenticateAsync(string email, string password);
        Task<User> RegisterAsync(string name, string email, string password, string role = "User");
    }
}
