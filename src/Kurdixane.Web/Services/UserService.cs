using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kurdixane.Web.Models;
using Kurdixane.Web.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kurdixane.Web.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _users;
    private readonly IPasswordHasher _hasher;

    public UserService(IRepository<User> users, IPasswordHasher hasher)
    {
        _users = users;
        _hasher = hasher;
    }

    public async Task<User?> ValidateCredentialsAsync(string email, string password)
    {
        var user = await _users.FirstOrDefaultAsync(u => u.Email == email && u.IsActive);
        if (user is null) return null;
        return _hasher.Verify(password, user.PasswordHash) ? user : null;
    }

    public async Task<(bool Success, string? Error, User? User)> RegisterAsync(string fullName, string email, string password)
    {
        if (await EmailExistsAsync(email))
            return (false, "Bu e-posta adresi zaten kayıtlı.", null);

        var user = new User
        {
            FullName = fullName,
            Email = email,
            PasswordHash = _hasher.Hash(password),
            Role = UserRole.Customer,
            IsActive = true
        };
        await _users.AddAsync(user);
        await _users.SaveChangesAsync();
        return (true, null, user);
    }

    public async Task<User?> GetByIdAsync(int id) => await _users.GetByIdAsync(id);

    public async Task<List<User>> GetAllAsync()
        => await _users.Query().OrderByDescending(u => u.Id).ToListAsync();

    public async Task<User> SaveAsync(User user, string? newPassword = null)
    {
        if (!string.IsNullOrWhiteSpace(newPassword))
            user.PasswordHash = _hasher.Hash(newPassword);

        if (user.Id == 0)
            await _users.AddAsync(user);
        else
            _users.Update(user);

        await _users.SaveChangesAsync();
        return user;
    }

    public async Task DeleteAsync(int id)
    {
        var e = await _users.GetByIdAsync(id);
        if (e is null) return;
        _users.Remove(e);
        await _users.SaveChangesAsync();
    }

    public async Task<bool> EmailExistsAsync(string email)
        => await _users.AnyAsync(u => u.Email == email);
}
