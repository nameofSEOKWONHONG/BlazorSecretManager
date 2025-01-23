using eXtensionSharp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace BlazorSecretManager.Infrastructure;

public class ApiAuthFilter : IAsyncActionFilter
{
    private readonly SecretDbContext _dbContext;

    public ApiAuthFilter(SecretDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.HttpContext.Request.Headers.TryGetValue("Authorization", out var token))
        {
            var exists = await _dbContext.Users.FirstOrDefaultAsync(m => m.UserKey == token.ToString());
            if (exists.xIsEmpty())
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }

        await next();
    }
}