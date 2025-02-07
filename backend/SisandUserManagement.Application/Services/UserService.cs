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

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return await _userRepository.GetByIdAsync(id);
    }

    public async Task<bool> UpdateUserAsync(Guid id, string name, string email)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return false;

        user.Name = name;
        user.Email = email;

        await _userRepository.UpdateAsync(user);
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
