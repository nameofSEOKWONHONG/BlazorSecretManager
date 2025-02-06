using MudBlazor;

namespace BlazorSecretManager.Hubs.Dtos;

public class ChatMessage
{
    public ChatBubblePosition Position { get; set; }
    public Color Color { get; set; }
    public string Email { get; set; }
    public string Message { get; set; }
    public string ReceivedDate { get; set; }
}

public class NoticeMessage
{
    public string UserId { get; set; }
    public string Type { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Extra { get; set; }
    public DateTime PublishDate { get; set; }
    public bool IsRead { get; set; }
}