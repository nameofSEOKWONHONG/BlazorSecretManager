using BlazorSecretManager.Services.Menu.Abstracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;

namespace BlazorSecretManager.Services.Menu;

public class MenuService : IMenuService
{
    private readonly AppDbContext _dbContext;
    private readonly HybridCache _hybridCache;

    public MenuService(AppDbContext dbContext, HybridCache hybridCache)
    {
        _dbContext = dbContext;
        _hybridCache = hybridCache;
    }
    
    public async Task<List<Entities.Menu>> GetMenuWithSubMenusAsync()
    {
        var options = new HybridCacheEntryOptions
        {
            Expiration = TimeSpan.FromHours(1),
            LocalCacheExpiration = TimeSpan.FromMinutes(30)
        };
        
        return await _hybridCache.GetOrCreateAsync("Menus", async (token) => await _dbContext.Menus.AsNoTracking()
            .OrderBy(m => m.Sort)
            .ToListAsync(token), options);
    }
}