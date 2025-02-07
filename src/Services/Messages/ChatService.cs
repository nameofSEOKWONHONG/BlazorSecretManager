using BlazorSecretManager.Entities;
using eXtensionSharp;
using Microsoft.EntityFrameworkCore;

namespace BlazorSecretManager.Services.Messages;

public interface IChatService
{
    Task<int> CreateRoom(string ownerId, string name, string[] attendUsers);
    Task<IReadOnlyList<ChatRoom>> GetRooms(string ownerId);
    
    Task<(int totalcount, IReadOnlyList<Chat>)> GetChats(int roomId, int pageNo = 1, int pageSize = 50);
    Task AddChat(int roomId, string fromUserId, string message);
}

public class ChatService : IChatService
{
    private readonly AppDbContext _dbContext;

    public ChatService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> CreateRoom(string ownerId, string name, string[] attendUsers)
    {
        var newItem = new ChatRoom()
        {
            OwnerId = ownerId,
            Name = name,
            AttendUsers = attendUsers.xJoin(","),
            CreatedAt = DateTime.Now
        };
        await _dbContext.ChatRooms.AddAsync(newItem);
        await _dbContext.SaveChangesAsync();
        
        return newItem.Id;
    }

    public async Task<IReadOnlyList<ChatRoom>> GetRooms(string ownerId)
    {
        var ownRooms = await _dbContext.ChatRooms.AsNoTracking()
            .Where(m => m.OwnerId == ownerId)
            .ToListAsync();
        var otherRooms = await _dbContext.ChatRooms.AsNoTracking()
            .Where(m => EF.Functions.Like(m.AttendUsers, $"%{ownerId}%"))
            .ToListAsync();
        return ownRooms.Concat(otherRooms).ToList();
    }

    public async Task<(int totalcount, IReadOnlyList<Chat>)> GetChats(int roomId, int pageNo = 1, int pageSize = 50)
    {
        var query = _dbContext.Chats.AsNoTracking()
            .Where(m => m.ChatRoomId == roomId);
        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(m => m.Id)
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return (total, items);
    }

    public async Task AddChat(int roomId, string fromUserId, string message)
    {   
        var fromUser   = await _dbContext.Users.AsNoTracking().Where(u => u.Id == fromUserId).FirstOrDefaultAsync();
        var item = new Chat { ChatRoomId = roomId,
            FromUserId = fromUser.Id,
            FromUserName = fromUser.UserName,
            Message = message, 
            CreatedAt = DateTime.Now};
        await _dbContext.Chats.AddAsync(item);
        await _dbContext.SaveChangesAsync();
    }
}