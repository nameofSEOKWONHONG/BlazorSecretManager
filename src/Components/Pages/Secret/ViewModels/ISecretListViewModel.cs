using MudComposite.ViewComponents.Composites.ListView;

namespace BlazorSecretManager.Components.Pages.Secret.ViewModels;

public interface ISecretListViewModel : IMudDataGridViewModel<Entities.Secret, SecretSearchModel>
{
}