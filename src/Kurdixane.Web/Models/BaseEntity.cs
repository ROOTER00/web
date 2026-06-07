using System;

namespace Kurdixane.Web.Models;

/// <summary>
/// Common audit fields shared by all persisted entities.
/// </summary>
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
