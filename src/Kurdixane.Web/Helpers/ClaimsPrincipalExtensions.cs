using System.Security.Claims;

namespace Kurdixane.Web.Helpers;

public static class ClaimsPrincipalExtensions
{
    public static int? GetUserId(this ClaimsPrincipal principal)
    {
        var value = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(value, out var id) ? id : null;
    }

    public static string? GetDisplayName(this ClaimsPrincipal principal)
        => principal.FindFirstValue(ClaimTypes.Name);
}
