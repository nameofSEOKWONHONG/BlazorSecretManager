using BlazorSecretManager.Entities;

namespace BlazorSecretManager.Services.Secrets.Abstracts;

public interface ISecretService
{
    Task<ValueTuple<int, List<Secret>>> GetSecrets(string title, string description, int pageNo = 1, int pageSize = 10);
    Task<Secret> GetSecret(int id);
    Task<int> CreateSecret(Secret secret);
    Task<bool> UpdateSecret(Secret secret);
    Task<bool> DeleteSecret(int id);
    Task<string> GetSecretUrl(int id);
}