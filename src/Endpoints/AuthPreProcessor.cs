using eXtensionSharp;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace BlazorSecretManager.Endpoints;

public class AuthPreProcessor<TRequest> : IPreProcessor<TRequest>
{
    public async Task PreProcessAsync(IPreProcessorContext<TRequest> context, CancellationToken ct)
    {
        var dbContext = context.HttpContext.Resolve<AppDbContext>();
        context.HttpContext.Request.Headers.TryGetValue("Authorization", out var token);
        if (token.xIsEmpty())
        {
            await context.HttpContext.Response.SendUnauthorizedAsync(cancellation: ct);
            return;
        }
        
        var exists = await dbContext.Users.FirstOrDefaultAsync(m => m.UserKey == token.ToString(), cancellationToken: ct);
        if (exists.xIsEmpty())
        {
            await context.HttpContext.Response.SendUnauthorizedAsync(cancellation: ct);
        }
    }
}