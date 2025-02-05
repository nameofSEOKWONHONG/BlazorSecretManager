using Microsoft.AspNetCore.SignalR;

namespace BlazorSecretManager.Hubs;

public class ChatHub : Hub
{
    private readonly AppDbContext _dbContext;
    public const string HubUrl = "/chat";
    public ChatHub(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task SendMessage(string from, string to, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", from, to, message, DateTime.Now);
    }
    
    // 그룹에 사용자 추가
    public async Task JoinGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Clients.Group(groupName).SendAsync("ReceiveGroupMessage", "System", $"{Context.ConnectionId} has joined the group {groupName}.");
    }

    // 그룹에서 사용자 제거
    public async Task LeaveGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        await Clients.Group(groupName).SendAsync("ReceiveGroupMessage", "System", $"{Context.ConnectionId} has left the group {groupName}.");
    }

    // 그룹에 메시지 보내기
    public async Task SendMessageToGroup(string groupName, string user, string message)
    {
        await Clients.Group(groupName).SendAsync("ReceiveGroupMessage", user, message);
    }
}