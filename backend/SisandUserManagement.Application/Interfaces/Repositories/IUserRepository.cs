using SisandUserManagement.Domain.Entities;

namespace SisandUserManagement.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUsernameAsync(string userName);
    Task<User?> GetByIdAsync(Guid id);
    Task<List<User>> GetAllAsync();
    Task<(List<Dictionary<string, object>>, int)> GetAllPaginatedAsync(int page, int pageSize, List<string>? fields = null, string? filter = null);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(Guid id);
}
