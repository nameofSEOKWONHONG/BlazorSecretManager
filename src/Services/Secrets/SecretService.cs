using BlazorSecretManager.Entities;
using BlazorSecretManager.Infrastructure;
using BlazorSecretManager.Services.Secrets.Abstracts;
using eXtensionSharp;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MudMvvMKit;

namespace BlazorSecretManager.Services.Secrets;

public class SecretService : ISecretService
{
    private readonly AppDbContext _dbContext;
    public SecretService(AppDbContext dbContext)
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

    public async Task<Results<Secret>> GetSecret(int id)
    {
        var item = await _dbContext.Secrets.AsNoTracking().FirstAsync(x => x.Id == id);
        return await Results<Secret>.SuccessAsync(item);
    }

    public async Task<Results<int>> AddOrUpdate(Secret secret)
    {
        var exists = await _dbContext.Secrets.FirstOrDefaultAsync(x => x.Id == secret.Id);
        if (exists.xIsEmpty())
        {
            var result = await CreateSecret(secret);
            return await Results<int>.SuccessAsync(result);
        }
        
        exists.Title = secret.Title.Trim();
        exists.Description = secret.Description.Trim();
        exists.Json = secret.Json.Trim();
        exists.UpdatedAt = DateTime.Now;
        
        _dbContext.Secrets.Update(exists);
        await _dbContext.SaveChangesAsync();

        return await Results<int>.SuccessAsync(exists.Id);
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


    public async Task<Results<bool>> DeleteSecret(int id)
    {
        var exists = await _dbContext.Secrets.FirstOrDefaultAsync(x => x.Id == id);
        if (exists.xIsEmpty()) return await Results<bool>.FailAsync("Secret not found");
        _dbContext.Secrets.Remove(exists);
        await _dbContext.SaveChangesAsync();
        
        return await Results<bool>.SuccessAsync();
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