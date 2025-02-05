using Microsoft.AspNetCore.SignalR;

namespace BlazorSecretManager.Hubs;

public class NoticeHub : Hub
{
    public const string HubUrl = "/notice";

    public async Task SendNotification(string userId, string type, string message)
    {
        await Clients.All.SendAsync("ReceiveNotification", userId, type, message);
    }
}