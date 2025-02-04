using System.Collections.Concurrent;
using eXtensionSharp;

namespace BlazorSecretManager.Infrastructure;

public interface IUserConnectionService
{
    void AddUser(UserSession session);
    void RemoveUser(string connectionId);
    bool AlreadyExists(string userId);
}

public class UserConnectionService : IUserConnectionService
{
    private ConcurrentDictionary<string, UserSession> _userSessions = new();
    
    public UserConnectionService()
    {
        
    }

    public void AddUser(UserSession session)
    {
        _userSessions.TryAdd(session.UserId, session);
    }

    public bool AlreadyExists(string userId)
    {
        return _userSessions.Count(m => m.Key == userId) > 0;
    }

    public void RemoveUser(string connectionId)
    {
        var exists = _userSessions.FirstOrDefault(m => m.Value.ConnectionId == connectionId);
        if (exists.Key.xIsNotEmpty())
        {
            _userSessions.TryRemove(exists.Key, out _);    
        }
    }
}