using BlazorSecretManager.Services.Messages;
using Microsoft.AspNetCore.SignalR;

namespace BlazorSecretManager.Hubs;

public class NoticeHub : Hub
{
    private readonly INotificationService _notificationService;
    public const string HubUrl = "/notice";

    public NoticeHub(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task SendNotification(string userId, string type, string message)
    {
        await Clients.All.SendAsync("ReceiveNotification", userId, type, message);
    }

    public async Task ConfirmNotification(int id)
    {
        await _notificationService.SetReadNotification(id);
    }
}