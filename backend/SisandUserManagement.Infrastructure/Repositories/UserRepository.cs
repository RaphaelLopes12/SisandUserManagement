using Microsoft.EntityFrameworkCore;
using SisandUserManagement.Application.Interfaces.Repositories;
using SisandUserManagement.Domain.Entities;
using SisandUserManagement.Infrastructure.Persistence;

namespace SisandUserManagement.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByUsernameAsync(string userName)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == userName);
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<(List<Dictionary<string, object>>, int)> GetAllPaginatedAsync(
        int page, int pageSize, List<string>? fields = null, string? filter = null)
    {
        var query = _context.Users.AsQueryable();

        if (!string.IsNullOrEmpty(filter))
        {
            query = query.Where(u =>
                u.Name.Contains(filter) ||
                u.Email.Contains(filter) ||
                u.Username.Contains(filter) ||
                u.Address.Contains(filter));
        }

        var totalUsers = await query.CountAsync();

        var usersList = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var users = usersList.Select(user =>
        {
            var result = new Dictionary<string, object>();

            if (fields == null || !fields.Any())
            {
                foreach (var property in typeof(User).GetProperties())
                {
                    result[property.Name] = property.GetValue(user);
                }
            }
            else
            {
                foreach (var field in fields)
                {
                    var property = typeof(User)
                        .GetProperties()
                        .FirstOrDefault(p => p.Name.Equals(field, StringComparison.OrdinalIgnoreCase));

                    if (property != null)
                    {
                        result[property.Name] = property.GetValue(user);
                    }
                }
            }

            return result;
        }).ToList();

        return (users, totalUsers);
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
