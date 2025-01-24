using BlazorSecretManager.Infrastructure;
using BlazorSecretManager.Services.Auth.Requests;
using MudComposite;

namespace BlazorSecretManager.Services.Auth.Abstracts;

public interface IAuthService
{
    Task<Results<string>> SignIn(string email, string password);
    Task<Results<bool>> SignUp(RegisterRequest request);
}