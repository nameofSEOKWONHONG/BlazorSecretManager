using System.Security.Claims;
using BlazorSecretManager.Entities;
using eXtensionSharp;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;
using MudComposite;
using MudComposite.Base;
using SecretManager;

namespace BlazorSecretManager.Services.Auth;

public interface IUserService
{
    Task<PaginatedResult<User>> GetUsers(string email, string name, int pageNo, int pageSize);
    Task<Results<User>> GetUser(string id);
    Task<Results<bool>> Remove(string id);
    Task<Results<bool>> Lock(string id);
}

public class UserService : IUserService
{
    private readonly AppDbContext _dbContext;
    private readonly AuthenticationStateProvider _provider;

    public UserService(AppDbContext dbContext, AuthenticationStateProvider provider)
    {
        _dbContext = dbContext;
        _provider = provider;
    }

    public async Task<PaginatedResult<User>> GetUsers(string email, string name, int pageNo, int pageSize)
    {
        var query = _dbContext.Users.AsNoTracking();
        if (email.xIsNotEmpty()) query = query.Where(m => m.Email.Contains(email));
        if (name.xIsNotEmpty()) query = query.Where(m => m.UserName.Contains(name));
        
        var total = await query.CountAsync();
        var result = await query.Skip(pageNo * pageSize).Take(pageSize).ToListAsync();
        return await PaginatedResult<User>.SuccessAsync(result, total, pageNo, pageSize);
    }

    public async Task<Results<User>> GetUser(string id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        return await Results<User>.SuccessAsync(user);
    }
    
    public async Task<Results<bool>> Remove(string id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
        return await Results<bool>.SuccessAsync();
    }

    public async Task<Results<bool>> Lock(string id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        user.LockoutEnabled = !user.LockoutEnabled;
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
        return await Results<bool>.SuccessAsync();
    }
}