using BlazorSecretManager;
using BlazorSecretManager.Components;
using BlazorSecretManager.Composites;
using BlazorSecretManager.Entities;
using BlazorSecretManager.Infrastructure;
using BlazorSecretManager.Services;
using BlazorSecretManager.Services.Auth;
using BlazorSecretManager.Services.Auth.Abstracts;
using BlazorSecretManager.Services.Menu;
using BlazorSecretManager.Services.Menu.Abstracts;
using BlazorSecretManager.Services.Secrets;
using BlazorSecretManager.Services.Secrets.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using MudBlazor;
using MudBlazor.Services;
using MudComposite;

#pragma warning disable EXTEXP0018

var builder = WebApplication.CreateBuilder(args);

#if DEBUG
DotNetEnv.Env.Load("./.env");
#endif 
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

#if DEBUG
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
#else
var connectionString = Environment.GetEnvironmentVariable("SQLITE_CONNECTION") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
#endif
builder.Services.AddDbContext<SecretDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<SecretDbContext>()
    ;

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;

    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 5000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});
builder.Services.AddMudComposite();

builder.Services.AddControllers();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISecretService, SecretService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, BlazorAuthorizationMiddlewareResultHandler>();
builder.Services.AddAuthenticationCore();
builder.Services.AddScoped<CircuitHandler, CustomCircuitHandler>();

builder.Services.AddScoped<ISecretComposite, SecretComposite>();

builder.Services.AddHybridCache(options =>
{
    // Maximum size of cached items
    options.MaximumPayloadBytes = 1024 * 1024 * 10; // 10MB
    options.MaximumKeyLength = 512;

    // Default timeouts
    options.DefaultEntryOptions = new HybridCacheEntryOptions
    {
        Expiration = TimeSpan.FromMinutes(30),
        LocalCacheExpiration = TimeSpan.FromMinutes(30)
    };
});
    
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapControllers();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

await using (var scope = app.Services.CreateAsyncScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SecretDbContext>();

    #if DEBUG

    await context.Database.MigrateAsync();

    #endif
    
    var any = await context.Menus.AnyAsync();
    if (!any)
    {
        var menus = new List<Menu>();
        menus.Add(new Menu() {Name = "Home", Description = "Home", Url = "/", Icon = Icons.Material.Filled.Home, Sort = 1});
        menus.Add(new Menu() {Name = "Secrets", Description = "List of secrets", Url = "/secrets", Icon = Icons.Material.Filled.Security, Sort = 2});
        await context.Menus.AddRangeAsync(menus);
        await context.SaveChangesAsync();
    }
}

app.Run();