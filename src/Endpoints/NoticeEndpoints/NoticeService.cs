using BlazorSecretManager.Entities;
using BlazorSecretManager.Hubs;
using BlazorSecretManager.Services.Messages;
using eXtensionSharp;
using Microsoft.AspNetCore.SignalR;

namespace BlazorSecretManager.Endpoints.NoticeEndpoints;

public class NoticeService : INoticeService
{
    private readonly IHubContext<NoticeHub> _hub;
    private readonly INotificationService _notificationService;

    public NoticeService(IHubContext<NoticeHub> hub, INotificationService notificationService)
    {
        _hub = hub;
        _notificationService = notificationService;
    }

    public async Task BroadcastNotice(Notification message)
    {
        await Task.Delay(1000);
        var id = await _notificationService.AddNotification(message);
        message.Id = id;
        await _hub.Clients.All.SendAsync("ReceiveNotification", message.UserId, message.Type, message.xSerialize());   
    }
}