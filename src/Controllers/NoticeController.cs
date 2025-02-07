using BlazorSecretManager.Entities;
using BlazorSecretManager.Hubs;
using BlazorSecretManager.Hubs.Dtos;
using BlazorSecretManager.Services.Messages;
using eXtensionSharp;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BlazorSecretManager.Controllers;

[AllowAnonymous]
[Route("api/[controller]")]
public class NoticeController : ControllerBase
{
    private readonly INoticeService _service;
    private readonly IBackgroundJobClient _backgroundJobClient;

    public NoticeController(INoticeService service,IBackgroundJobClient backgroundJobClient)
    {
        _service = service;
        _backgroundJobClient = backgroundJobClient;
    }
    
    [HttpPost]
    public IActionResult SendNotice([FromBody]Notification notification)
    {
        _backgroundJobClient.Enqueue(() => _service.BroadcastNotice(notification));
        return Ok();
    }
}

public interface INoticeService
{
    Task BroadcastNotice(Notification message);
}

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