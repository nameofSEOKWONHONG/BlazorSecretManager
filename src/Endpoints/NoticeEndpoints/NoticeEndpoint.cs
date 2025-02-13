using BlazorSecretManager.Entities;
using FastEndpoints;
using Hangfire;

namespace BlazorSecretManager.Endpoints.NoticeEndpoints;

public class NoticeEndpoint : Endpoint<Notification>
{
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly INoticeService _service;

    public NoticeEndpoint(IBackgroundJobClient backgroundJobClient, INoticeService service)
    {
        _backgroundJobClient = backgroundJobClient;
        _service = service;
    }
    
    public override void Configure()
    {
        Post("/api/notice");
    }

    public override async Task HandleAsync(Notification req, CancellationToken ct)
    {
        _backgroundJobClient.Enqueue(() => _service.BroadcastNotice(req));
        await SendOkAsync(ct);
    }
}