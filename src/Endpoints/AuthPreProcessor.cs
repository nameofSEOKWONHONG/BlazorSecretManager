using eXtensionSharp;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace BlazorSecretManager.Endpoints;

public class AuthPreProcessor<TRequest> : IPreProcessor<TRequest>
{
    public async Task PreProcessAsync(IPreProcessorContext<TRequest> context, CancellationToken ct)
    {
        var dbContext = context.HttpContext.Resolve<AppDbContext>();
        context.HttpContext.Request.Headers.TryGetValue("apiKey", out var secretKey);
        if (secretKey.xIsEmpty())
        {
            await context.HttpContext.Response.SendUnauthorizedAsync(cancellation: ct);
            return;
        }
        
        var trimSecretKey = secretKey.ToString().Replace("apiKey", "").Trim();
        
        var exists = await dbContext.Users.FirstOrDefaultAsync(m => m.UserKey == trimSecretKey, cancellationToken: ct);
        if (exists.xIsEmpty())
        {
            await context.HttpContext.Response.SendUnauthorizedAsync(cancellation: ct);
        }
    }
}