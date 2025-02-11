using MudMvvMKit.Base;

namespace BlazorSecretManager.Infrastructure;

public class UserSession
{
    public string UserId { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public string UserKey { get; set; }
    public string Phone { get; set; }
    public string ConnectionId { get; set; }
}