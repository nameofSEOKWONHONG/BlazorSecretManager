using BlazorSecretManager.Infrastructure;
using MudComposite.ViewComponents.Composites.ListView;

namespace BlazorSecretManager.Components.Pages.User.ViewModels;

public interface IUserListViewModel : IMudListViewModel<Entities.User, UserSearchModel>
{
    UserSession UserSession { get; set; }
}