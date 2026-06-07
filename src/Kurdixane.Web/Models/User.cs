using System.ComponentModel.DataAnnotations;

namespace Kurdixane.Web.Models;

public enum UserRole
{
    Customer = 0,
    Admin = 1
}

public class User : BaseEntity
{
    [Required, StringLength(200)]
    public string FullName { get; set; } = string.Empty;

    [Required, StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [StringLength(40)]
    public string? Phone { get; set; }

    public UserRole Role { get; set; } = UserRole.Customer;
    public bool IsActive { get; set; } = true;
}
