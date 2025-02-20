using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace BlazorSecretManager;

public class OpenApiSecuritySchemeTransformer
    : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        document.Info.Title = "Secret Manager API";
        document.Info.Description = "Secret Manager API";
        document.Info.Contact = new OpenApiContact
        {
            Name = "Secret Manager API",
            Email = "h20913@gmail.com",
            Url = new Uri("http://localhost:5000")
        };

        var securitySchema =
            new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "JWT Authorization header using the Bearer scheme."
            };

        var securityRequirement =
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    []
                }
            };
        
        document.SecurityRequirements.Add(securityRequirement);
        document.Components = new OpenApiComponents()
        {
            SecuritySchemes = new Dictionary<string, OpenApiSecurityScheme>()
            {
                { "Bearer", securitySchema }
            }
        };
        return Task.CompletedTask;
    }
}