using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kurdixane.Web.Models;
using Kurdixane.Web.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kurdixane.Web.Services;

public class SettingService : ISettingService
{
    private readonly IRepository<SiteSetting> _settings;

    public SettingService(IRepository<SiteSetting> settings)
    {
        _settings = settings;
    }

    public async Task<Dictionary<string, string?>> GetAllAsync()
        => await _settings.Query().ToDictionaryAsync(s => s.Key, s => s.Value);

    public async Task<string?> GetAsync(string key)
    {
        var setting = await _settings.FirstOrDefaultAsync(s => s.Key == key);
        return setting?.Value;
    }

    public async Task SetAsync(string key, string? value)
    {
        var setting = await _settings.Query(tracking: true).FirstOrDefaultAsync(s => s.Key == key);
        if (setting is null)
        {
            await _settings.AddAsync(new SiteSetting { Key = key, Value = value });
        }
        else
        {
            setting.Value = value;
            _settings.Update(setting);
        }
        await _settings.SaveChangesAsync();
    }

    public async Task SetManyAsync(Dictionary<string, string?> values)
    {
        foreach (var kv in values)
        {
            var setting = await _settings.Query(tracking: true).FirstOrDefaultAsync(s => s.Key == kv.Key);
            if (setting is null)
                await _settings.AddAsync(new SiteSetting { Key = kv.Key, Value = kv.Value });
            else
            {
                setting.Value = kv.Value;
                _settings.Update(setting);
            }
        }
        await _settings.SaveChangesAsync();
    }

    public async Task<List<SiteSetting>> GetSettingEntitiesAsync()
        => await _settings.Query().OrderBy(s => s.Group).ThenBy(s => s.Key).ToListAsync();
}
