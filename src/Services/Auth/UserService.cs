using System.Security.Claims;
using BlazorSecretManager.Entities;
using BlazorSecretManager.Infrastructure;
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
    Task<List<ChatUserModel>> GetUsers();
    Task<Results<User>> GetUser(string id);
    Task<Results<bool>> Remove(string id);
    Task<Results<bool>> Lock(string id);
    Task<UserSession> GetUserSession();
}

public class ChatUserModel
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
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

    public async Task<List<ChatUserModel>> GetUsers()
    {
        var users = await _dbContext.Users.AsNoTracking().Select(m => new ChatUserModel()
        {
            Id = m.Id,
            Email = m.Email,
            Name = m.UserName
        }).ToListAsync();

        return users;
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
    
    public async Task<UserSession> GetUserSession()
    {
        var state = await _provider.GetAuthenticationStateAsync();
        var userId = state.User.Claims.First(m => m.Type == ClaimTypes.NameIdentifier).Value;
        var email = state.User.Claims.First(m => m.Type == ClaimTypes.Email).Value;
        var name = state.User.Claims.First(m => m.Type == ClaimTypes.Name).Value;
        var key = state.User.Claims.First(m => m.Type == ClaimTypes.PrimarySid).Value;
        var phone = state.User.Claims.First(m => m.Type == ClaimTypes.MobilePhone).Value;
        var role = state.User.Claims.First(m => m.Type == ClaimTypes.Role).Value;
        return new UserSession()
        {
            UserId = userId,
            Email = email,
            Name = name,
            Role = role,
            UserKey = key,
            Phone = phone,
        };        
    }
}