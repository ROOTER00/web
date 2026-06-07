using System.Collections.Generic;
using System.Threading.Tasks;
using Kurdixane.Web.Models;

namespace Kurdixane.Web.Services;

public interface ISettingService
{
    Task<Dictionary<string, string?>> GetAllAsync();
    Task<string?> GetAsync(string key);
    Task SetAsync(string key, string? value);
    Task SetManyAsync(Dictionary<string, string?> values);
    Task<List<SiteSetting>> GetSettingEntitiesAsync();
}
