using BlazorSecretManager.Entities;

namespace BlazorSecretManager.Services.Auth.Abstracts;

public interface IUserRepository
{
    Task<bool> CreateUser(User insertUser);
    Task<User> GetUser(string id);
    Task<User> GetUserByEmail(string email);
}