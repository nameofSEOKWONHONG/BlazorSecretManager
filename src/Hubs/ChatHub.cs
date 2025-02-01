using Microsoft.AspNetCore.SignalR;

namespace BlazorSecretManager.Hubs;

public class ChatHub : Hub
{
    public ChatHub()
    {
        
    }
    
    public async Task SendMessage(string user, string message)
    {
        // 모든 클라이언트에 메시지 전송
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
    
    // 그룹에 사용자 추가
    public async Task JoinGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Clients.Group(groupName).SendAsync("ReceiveMessage", "System", $"{Context.ConnectionId} has joined the group {groupName}.");
    }

    // 그룹에서 사용자 제거
    public async Task LeaveGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        await Clients.Group(groupName).SendAsync("ReceiveMessage", "System", $"{Context.ConnectionId} has left the group {groupName}.");
    }

    // 그룹에 메시지 보내기
    public async Task SendMessageToGroup(string groupName, string user, string message)
    {
        await Clients.Group(groupName).SendAsync("ReceiveMessage", user, message);
    }
}