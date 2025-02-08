using SisandUserManagement.Domain.Entities;

namespace SisandUserManagement.Application.Interfaces.Services;

public interface IUserService
{
    Task<User?> GetAuthenticatedUserAsync(string userId);
    Task<List<User>> GetAllUsersAsync();
    Task<(List<Dictionary<string, object>>, int)> GetAllUsersPaginatedAsync(int page, int pageSize, List<string>? fields, string? filter);
    Task<User?> GetUserByIdAsync(Guid id);
    Task<bool> UpdateUserAsync(Guid id, User updatedUser);
    Task<bool> DeleteUserAsync(Guid id);
}
