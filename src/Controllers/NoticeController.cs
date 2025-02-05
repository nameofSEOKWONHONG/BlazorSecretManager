using BlazorSecretManager.Hubs;
using BlazorSecretManager.Hubs.Dtos;
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
    private readonly IBackgroundJobClient _backgroundJobClient;

    public NoticeController(IBackgroundJobClient backgroundJobClient)
    {
        _backgroundJobClient = backgroundJobClient;
    }
    
    [HttpPost]
    public IActionResult SendNotice()
    {
        var message = new NoticeMessage()
        {
            Title = "test",
            Content = "test",
            Extra = "test",
            PublishDate = DateTime.Now,
            IsRead = false,
        };
        _backgroundJobClient.Enqueue<NoticeService>(m => m.BroadcastNotice(message));
        return Ok();
    }
}

public interface INoticeService
{
    Task BroadcastNotice(NoticeMessage message);
}

public class NoticeService : INoticeService
{
    private readonly IHubContext<NoticeHub> _hub;

    public NoticeService(IHubContext<NoticeHub> hub)
    {
        _hub = hub;
    }

    public async Task BroadcastNotice(NoticeMessage message)
    {
        await Task.Delay(1000);
        await _hub.Clients.All.SendAsync("ReceiveNotification", string.Empty, "A", message.xSerialize());   
    }
}