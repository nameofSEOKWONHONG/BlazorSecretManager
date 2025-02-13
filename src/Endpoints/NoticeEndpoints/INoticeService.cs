using BlazorSecretManager.Entities;

namespace BlazorSecretManager.Endpoints.NoticeEndpoints;

public interface INoticeService
{
    Task BroadcastNotice(Notification message);
}
