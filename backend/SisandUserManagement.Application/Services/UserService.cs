using SisandUserManagement.Application.Interfaces.Repositories;
using SisandUserManagement.Application.Interfaces.Services;
using SisandUserManagement.Domain.Entities;

namespace SisandUserManagement.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> GetAuthenticatedUserAsync(string userId)
    {
        if (!Guid.TryParse(userId, out var guid))
            return null;

        return await _userRepository.GetByIdAsync(guid);
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllAsync();
    }

    public async Task<(List<Dictionary<string, object>>, int)> GetAllUsersPaginatedAsync(int page, int pageSize, List<string>? fields, string? filter)
    {
        return await _userRepository.GetAllPaginatedAsync(page, pageSize, fields, filter);
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return await _userRepository.GetByIdAsync(id);
    }

    public async Task<bool> UpdateUserAsync(Guid id, User updatedUser)
    {
        var existingUser = await _userRepository.GetByIdAsync(id);
        if (existingUser == null) return false;

        existingUser.Name = updatedUser.Name;
        existingUser.Email = updatedUser.Email;
        existingUser.Username = updatedUser.Username;
        existingUser.Address = updatedUser.Address;
        existingUser.BirthDate = updatedUser.BirthDate;
        existingUser.PhoneNumber = updatedUser.PhoneNumber;
        existingUser.Role = updatedUser.Role;
        existingUser.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
            TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));

        await _userRepository.UpdateAsync(existingUser);
        return true;
    }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return false;

        await _userRepository.DeleteAsync(id);
        return true;
    }
}
