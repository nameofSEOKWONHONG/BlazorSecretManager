using BlazorSecretManager.Entities;
using BlazorSecretManager.Infrastructure;
using MudComposite;

namespace BlazorSecretManager.Services.Secrets.Abstracts;

public interface ISecretService
{
    Task<ValueTuple<int, List<Secret>>> GetSecrets(string title, string description, int pageNo = 1, int pageSize = 10);
    Task<Results<Secret>> GetSecret(int id);
    Task<Results<int>> AddOrUpdate(Secret secret);
    Task<bool> UpdateSecret(Secret secret);
    Task<Results<bool>> DeleteSecret(int id);
    Task<string> GetSecretUrl(int id);
}