using BlazorSecretManager.Entities;
using BlazorSecretManager.Infrastructure;
using BlazorSecretManager.Services.Auth.Abstracts;
using BlazorSecretManager.Services.Auth.Requests;
using eXtensionSharp;
using Microsoft.AspNetCore.Identity;

namespace BlazorSecretManager.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AuthService(IUserRepository userRepository,
        IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<string> SignIn(string email, string password)
    {
        var user = await _userRepository.GetUser(email);
        var valid = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, password);
        if (valid == PasswordVerificationResult.Failed) return string.Empty;
        
        var expire = DateTime.UtcNow.AddDays(1);
        return JwtGenerator.GenerateJwtToken(expire, user.Id,  user.Email, user.UserName, user.UserKey, user.PhoneNumber,user.RoleName);
    }

    public async Task<bool> SignUp(RegisterRequest request)
    {
        if (request.Password != request.ConfirmPassword) return false;
        var exists = await _userRepository.GetUser(request.Email);
        
        if (exists.xIsNotEmpty()) return false;

        var newItem = new User()
        {
            Email = request.Email,
            NormalizedEmail = request.Email.ToUpper(),
            UserName = request.Name,
            NormalizedUserName = request.Name.ToUpper(),
            PhoneNumber = request.PhoneNumber,
            RoleName = request.RoleName,
            UserKey = Guid.NewGuid().ToString("N"),
            PhoneNumberConfirmed = true,
            EmailConfirmed = true,
        };
        
        var hashPassword = _passwordHasher.HashPassword(newItem, request.Password);
        newItem.PasswordHash = hashPassword;

        await this._userRepository.CreateUser(newItem);
        return true;
    }
}