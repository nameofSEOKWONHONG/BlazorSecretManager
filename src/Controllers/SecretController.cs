using BlazorSecretManager.Infrastructure;
using eXtensionSharp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorSecretManager.Controllers;

[AllowAnonymous]
[TypeFilter<ApiAuthFilter>]
[Route("api/[controller]")]
public class SecretController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public SecretController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("{key}/{id}")]
    public async Task<IActionResult> GetJson(string key, int id)
    {
        var item = await _dbContext.Secrets.FirstOrDefaultAsync(m => m.Id == id);
        if (item.xIsEmpty()) return NotFound();
        if (item.SecretKey != key) return Unauthorized();
        return Ok(item.Json);
    }
}