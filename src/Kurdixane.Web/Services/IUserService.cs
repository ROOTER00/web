using System.Collections.Generic;
using System.Threading.Tasks;
using Kurdixane.Web.Models;

namespace Kurdixane.Web.Services;

public interface IUserService
{
    Task<User?> ValidateCredentialsAsync(string email, string password);
    Task<(bool Success, string? Error, User? User)> RegisterAsync(string fullName, string email, string password);
    Task<User?> GetByIdAsync(int id);
    Task<List<User>> GetAllAsync();
    Task<User> SaveAsync(User user, string? newPassword = null);
    Task DeleteAsync(int id);
    Task<bool> EmailExistsAsync(string email);
}
