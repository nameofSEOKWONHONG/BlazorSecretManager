using MudComposite.ViewComponents.Composites.ListView;

namespace BlazorSecretManager.Components.Pages.User.ViewModels;

public interface IUserListViewModel : IMudDataGridViewModel<Entities.User, UserSearchModel>
{
    
}