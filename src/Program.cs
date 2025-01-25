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
builder.Services.AddMudSecretManager(() => builder.Configuration);
    
var app = builder.Build();
app.UseMudSecretManager();

await using (var scope = app.Services.CreateAsyncScope())
{
    #if SQLITE
    if (!Path.Exists("./data"))
    {
        Directory.CreateDirectory("./data");
    }
    
    if (!File.Exists("./data/app.db"))
    {
        File.Create("./data/app.db");
    }    
    #endif
    
    var initializer = scope.ServiceProvider.GetRequiredService<ProgramInitializer>();
    await initializer.InitializeAsync();
}

app.Run();