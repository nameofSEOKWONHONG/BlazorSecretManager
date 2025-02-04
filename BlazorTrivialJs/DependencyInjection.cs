using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorTrivialJs;

public static class DependencyInjection
{
    public static void AddTrivialJs(this IServiceCollection services)
    {
        services.AddScoped<ITrivialJs, TrivialJs>();
    }
}