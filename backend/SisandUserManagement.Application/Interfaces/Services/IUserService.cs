using SisandUserManagement.Domain.Entities;

namespace SisandUserManagement.Application.Interfaces.Services;

public interface IUserService
{
    Task<User?> GetAuthenticatedUserAsync(string userId);
    Task<List<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(Guid id);
    Task<bool> UpdateUserAsync(Guid id, string name, string email);
    Task<bool> DeleteUserAsync(Guid id);
}
