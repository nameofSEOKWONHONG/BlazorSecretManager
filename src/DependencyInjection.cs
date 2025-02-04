using System.Diagnostics.CodeAnalysis;
using BlazorSecretManager.Components;
using BlazorSecretManager.Components.Pages.Secret.ViewModels;
using BlazorSecretManager.Components.Pages.User.ViewModels;
using BlazorSecretManager.Entities;
using BlazorSecretManager.Infrastructure;
using BlazorSecretManager.Services.Auth;
using BlazorSecretManager.Services.Auth.Abstracts;
using BlazorSecretManager.Services.Menu;
using BlazorSecretManager.Services.Menu.Abstracts;
using BlazorSecretManager.Services.Secrets;
using BlazorSecretManager.Services.Secrets.Abstracts;
using BlazorTrivialJs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using MudBlazor;
using MudBlazor.Services;
using MudComposite;

namespace BlazorSecretManager;

public static class DependencyInjection
{
    [Experimental("EXTEXP0018")]
    public static void AddMudSecretManager(this IServiceCollection services, Func<IConfiguration> config)
    {
        #if DEBUG
        DotNetEnv.Env.Load("./.env");
        #endif 
        // Add services to the container.
        services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddCircuitOptions(options => options.DetailedErrors = true);

        var configuration = config();

        #if DEBUG
        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                               throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        #else
        var connectionString = Environment.GetEnvironmentVariable("SQLITE_CONNECTION") ??
                               throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        #endif

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(connectionString));
        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            ;

        services.AddMudServices(config =>
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
        services.AddMudComposite();

        services.AddControllers();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ISecretService, SecretService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IMenuService, MenuService>();
        services.AddScoped<CustomAuthStateProvider>();
        services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
        services.AddScoped<CircuitHandler, CustomCircuitHandler>();
        services.AddSingleton<IAuthorizationMiddlewareResultHandler, BlazorAuthorizationMiddlewareResultHandler>();
        services.AddSingleton<IUserConnectionService, UserConnectionService>();
        services.AddAuthenticationCore();

        services.AddScoped<ISecretListViewModel, SecretListViewModel>();
        services.AddScoped<IUserListViewModel, UserListViewModel>();

        services.AddHybridCache(options =>
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

        services.AddTransient<ProgramInitializer>();
        
        services.AddScoped<AppState>();
        services.AddScoped<ITrivialJs, TrivialJs>();
    }

    public static void UseMudSecretManager(this WebApplication app)
    {
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
    }
}