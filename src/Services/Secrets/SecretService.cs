using BlazorSecretManager.Entities;
using BlazorSecretManager.Services.Secrets.Abstracts;
using eXtensionSharp;
using Microsoft.EntityFrameworkCore;

namespace BlazorSecretManager.Services.Secrets;

public class SecretService : ISecretService
{
    private readonly SecretDbContext _dbContext;
    public SecretService(SecretDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ValueTuple<int, List<Secret>>> GetSecrets(string title, string description, int pageNo = 1, int pageSize = 10)
    {
        var query = _dbContext.Secrets.AsNoTracking();
        
        if(title.xIsNotEmpty()) query = query.Where(x => x.Title.Contains(title));
        if(description.xIsNotEmpty()) query = query.Where(x => x.Description.Contains(description));
        
        var total = await query.CountAsync();
        var list = await _dbContext.Secrets.AsNoTracking()
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return new ValueTuple<int, List<Secret>>(total, list);
    }

    public async Task<Secret> GetSecret(int id)
    {
        return await _dbContext.Secrets.AsNoTracking().FirstAsync(x => x.Id == id);
    }
    
    public async Task<int> CreateSecret(Secret secret)
    {
        var exists = await _dbContext.Secrets.FirstOrDefaultAsync(x => x.Id == secret.Id);
        if (exists.xIsNotEmpty()) return 0;
        
        secret.SecretKey = Guid.NewGuid().ToString("N");
        secret.CreatedAt = DateTime.Now;

        await _dbContext.Secrets.AddAsync(secret);
        await _dbContext.SaveChangesAsync();
        
        return secret.Id;
    }

    public async Task<bool> UpdateSecret(Secret secret)
    {
        var exists = await _dbContext.Secrets.FirstOrDefaultAsync(x => x.Id == secret.Id);
        if (exists.xIsEmpty()) return false;
        
        exists.Title = secret.Title.Trim();
        exists.Description = secret.Description.Trim();
        exists.Json = secret.Json.Trim();
        exists.UpdatedAt = DateTime.Now;
        
        _dbContext.Secrets.Update(exists);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteSecret(int id)
    {
        var exists = await _dbContext.Secrets.FirstOrDefaultAsync(x => x.Id == id);
        if (exists.xIsEmpty()) return false;
        _dbContext.Secrets.Remove(exists);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<string> GetSecretUrl(int id)
    {
        var exists = await _dbContext.Secrets.FirstOrDefaultAsync(x => x.Id == id);
        if (exists.xIsEmpty()) return string.Empty;
        var host = Environment.GetEnvironmentVariable("HOST");
        var url = $"{host}/api/secret/{exists.SecretKey}/{id}";
        return url;
    }
}