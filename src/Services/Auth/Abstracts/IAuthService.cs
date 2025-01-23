using BlazorSecretManager.Services.Auth.Requests;

namespace BlazorSecretManager.Services.Auth.Abstracts;

public interface IAuthService
{
    Task<string> SignIn(string email, string password);
    Task<bool> SignUp(RegisterRequest request);
}