using BlazorSecretManager.Entities;
using eXtensionSharp;
using Microsoft.EntityFrameworkCore;

namespace BlazorSecretManager.Services.Messages;

public interface INotificationService
{
    Task<int> AddNotification(Notification notification);
    Task<List<Notification>> GetNotifications(string userId);
    Task SetReadNotification(int id);
}

public class NotificationService : INotificationService
{
    private readonly AppDbContext _dbContext;

    public NotificationService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> AddNotification(Notification notification)
    {
        await _dbContext.Notifications.AddAsync(notification);
        await _dbContext.SaveChangesAsync();
        return notification.Id;
    }

    public async Task<List<Notification>> GetNotifications(string userId)
    {
        return await _dbContext.Notifications.AsNoTracking()
            .Where(m => m.UserId == userId && !m.IsRead)
            .ToListAsync();
    }

    public async Task SetReadNotification(int id)
    {
        var exists = await _dbContext.Notifications.FirstOrDefaultAsync(m => m.Id == id);
        if (exists.xIsNotEmpty())
        {
            exists.IsRead = true;
            await _dbContext.SaveChangesAsync();
        }
    }
}