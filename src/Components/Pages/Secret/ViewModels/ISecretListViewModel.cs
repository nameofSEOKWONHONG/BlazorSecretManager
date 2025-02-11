using MudMvvMKit.ViewComponents.ViewModels.ListView;

namespace BlazorSecretManager.Components.Pages.Secret.ViewModels;

public interface ISecretListViewModel : IMudListViewModel<Entities.Secret, SecretSearchModel>
{
}