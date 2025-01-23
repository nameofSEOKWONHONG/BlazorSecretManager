namespace BlazorSecretManager.Services.Auth.Dtos;

public class UserSession
{
    public string UserId { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
}