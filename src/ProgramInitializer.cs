using BlazorSecretManager.Entities;
using Microsoft.EntityFrameworkCore;
using MudBlazor;

namespace BlazorSecretManager;

public class ProgramInitializer
{
    private readonly AppDbContext _dbContext;

    public ProgramInitializer(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task InitializeAsync()
    {
        await _dbContext.Database.MigrateAsync();
    
        await _dbContext.Database.ExecuteSqlAsync($"delete from Menus");
        await _dbContext.SaveChangesAsync();
    
        var menus = new List<Menu>();
        menus.Add(new Menu() {Name = "Home", Description = "Home", Url = "/", Icon = Icons.Material.Filled.Home, Sort = 1});
        menus.Add(new Menu() {Name = "Users", Description = "List of users", Url = "/users", Icon = Icons.Material.Filled.Person, Sort = 2});
        menus.Add(new Menu() {Name = "Secrets", Description = "List of secrets", Url = "/secrets", Icon = Icons.Material.Filled.Security, Sort = 3});
        await _dbContext.Menus.AddRangeAsync(menus);
        await _dbContext.SaveChangesAsync();
    }
}