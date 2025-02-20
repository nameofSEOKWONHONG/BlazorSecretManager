using eXtensionSharp;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace BlazorSecretManager.Endpoints.SecretEndpoints;

public class GetSecretRequest
{
    public string Key { get; set; }
    public int Id { get; set; }
}
public class SecretEndpoint : Endpoint<GetSecretRequest>
{
    private readonly AppDbContext _dbContext;

    public SecretEndpoint(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public override void Configure()
    {
        Get("/api/secret");
        PreProcessor<AuthPreProcessor<GetSecretRequest>>();
    }

    public override async Task HandleAsync(GetSecretRequest req, CancellationToken ct)
    {
        var item = await _dbContext.Secrets.Where(m => m.Id == req.Id && m.SecretKey == req.Key)
            .FirstOrDefaultAsync(ct);
        if (item.xIsEmpty())
        {
            await SendNotFoundAsync(ct);
        }

        this.Response = item.Json;
    }
}