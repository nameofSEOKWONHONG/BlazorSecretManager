using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;
using BlazorSecretManager.Services.Auth.Abstracts;
using eXtensionSharp;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SecretManager;

namespace BlazorSecretManager.Infrastructure;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ProtectedSessionStorage _protectedSessionStore;
    private readonly NavigationManager _navigationManager;
    private readonly IUserRepository _userRepository;

    public CustomAuthStateProvider(ProtectedSessionStorage protectedSessionStore,
        NavigationManager navigationManager,
        IUserRepository userRepository)
    {
        _protectedSessionStore = protectedSessionStore;
        _navigationManager = navigationManager;
        _userRepository = userRepository;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // if (_navigationManager.Uri.StartsWith("http"))
        // {
            try
            {
                var tokenString = await _protectedSessionStore.GetAsync<string>(Constants.JwtCacheKey);

                if (IsTokenValid(tokenString.Value) == false)
                {
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                var jwtToken = new JwtSecurityToken(tokenString.Value);

                var identity = new ClaimsIdentity(jwtToken.Claims, Constants.JwtCacheKey);
                var claimsPrincipal = new ClaimsPrincipal(identity);
                var id = claimsPrincipal.Claims.First(m => m.Type == ClaimTypes.NameIdentifier)?.Value;
                if (id.xIsNotEmpty())
                {
                    var user = await _userRepository.GetUser(id);
                    if (user.xIsEmpty())
                    {
                        await _protectedSessionStore.DeleteAsync(Constants.JwtCacheKey);
                        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));                        
                    }
                }
                return new AuthenticationState(claimsPrincipal);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        // }
        // else
        // {
        //     Console.WriteLine("Server");
        //     //todo:...
        // }
        
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }
    

    public event Action OnChange;
    public void NotifyUserAuthentication(string tokenString)
    {
        var jwtToken = new JwtSecurityToken(tokenString);
        var identity = new ClaimsIdentity(jwtToken.Claims, Constants.JwtCacheKey);
        var user = new ClaimsPrincipal(identity);
        var authState = Task.FromResult(new AuthenticationState(user));
        NotifyAuthenticationStateChanged(authState);
        OnChange?.Invoke();
    }

    public void NotifyUserAuthentication(ClaimsPrincipal principal)
    {
        var identity = new ClaimsIdentity(principal.Claims, Constants.JwtCacheKey);
        var user = new ClaimsPrincipal(identity);
        var authState = Task.FromResult(new AuthenticationState(user));
        NotifyAuthenticationStateChanged(authState);
        OnChange?.Invoke();        
    }

    public void NotifyUserLogout()
    {
        var authState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
        NotifyAuthenticationStateChanged(authState);
        OnChange?.Invoke();
    }

    private bool IsTokenValid(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return false;
        }
        var tokenHandler = new JwtSecurityTokenHandler();

        if (!tokenHandler.CanReadToken(token))
            return false;

        var jwtToken = tokenHandler.ReadJwtToken(token);
        if (jwtToken.xIsNotEmpty())
        {
            if (jwtToken.Payload.Expiration.xIsNotEmpty())
            {
                var expiration = jwtToken.Payload.Expiration;
                var expirationDate = DateTimeOffset.FromUnixTimeSeconds(expiration.GetValueOrDefault()).DateTime;
                return expirationDate > DateTime.UtcNow;                
            }
        }
        
        return false;
    }
}

