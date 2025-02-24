using System.Diagnostics.CodeAnalysis;
using BlazorSecretManager.Components;
using BlazorSecretManager.Components.Pages.Secret.ViewModels;
using BlazorSecretManager.Components.Pages.User.ViewModels;
using BlazorSecretManager.Endpoints.NoticeEndpoints;
using BlazorSecretManager.Entities;
using BlazorSecretManager.Hubs;
using BlazorSecretManager.Infrastructure;
using BlazorSecretManager.Services.Auth;
using BlazorSecretManager.Services.Auth.Abstracts;
using BlazorSecretManager.Services.Menu;
using BlazorSecretManager.Services.Menu.Abstracts;
using BlazorSecretManager.Services.Messages;
using BlazorSecretManager.Services.Secrets;
using BlazorSecretManager.Services.Secrets.Abstracts;
using BlazorTrivialJs;
using Brism;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using MudBlazor;
using MudBlazor.Extensions;
using MudBlazor.Services;
using MudMvvMKit;
using NSwag;

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
        
        services.AddCors();
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        services.AddOpenApi(options =>
        {
            options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0;
        });

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
        services.AddMudServicesWithExtensions(c => c.WithoutAutomaticCssLoading());
        services.AddMudMarkdownServices();

        services.AddControllers();
        services.AddFastEndpoints()
            .SwaggerDocument(o =>
            {
                o.EnableJWTBearerAuth = false;
                o.DocumentSettings = s =>
                {
                    s.DocumentName = "Initial-Release";
                    s.Title = "Web API";
                    s.Version = "v1.0";
                    s.AddAuth("ApiKey", new()
                    {
                        Name = "api_key",
                        In = OpenApiSecurityApiKeyLocation.Header,
                        Type = OpenApiSecuritySchemeType.ApiKey
                    });
                };
            });

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
        services.AddBrism();

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
        services.AddTrivialJs();
        
        services.AddHangfire(config => config.UseMemoryStorage());
        services.AddHangfireServer();

        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<INoticeService, NoticeService>();
        services.AddScoped<IChatService, ChatService>();
        services.AddScoped<IFileService, FileService>();
    }

    public static void UseMudSecretManager(this WebApplication app)
    {
        app.MapOpenApi();
        
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
        app.UseFastEndpoints()
            .UseSwaggerGen(); //add this
        
        app.UseAntiforgery();

        app.UseHangfireDashboard();
        
        app.Use(MudExWebApp.MudExMiddleware);
        app.MapControllers();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();
        app.MapHub<ChatHub>(ChatHub.HubUrl);
        app.MapHub<NoticeHub>(NoticeHub.HubUrl);
    }
}