using BlazorSecretManager.Services.Auth;
using eXtensionSharp;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using MudComposite;
using MudComposite.ViewComponents.Composites.ListView;

namespace BlazorSecretManager.Components.Pages.User.ViewModels;

public class UserListViewModel : MudDataGridViewModel<Entities.User, UserSearchModel>, IUserListViewModel
{
    private readonly IUserService _userService;

    public UserListViewModel(IDialogService dialogService, ISnackbar snackbar, NavigationManager navigationManager, AuthenticationStateProvider authenticationStateProvider,
        IUserService userService) : base(dialogService, snackbar, navigationManager, authenticationStateProvider)
    {
        _userService = userService;
    }

    public override void Initialize(MudDataGrid<Entities.User> dataGrid)
    {
        base.Initialize(dataGrid);
        
        this.OnServerReload = async (state) =>
        {
            var result = await _userService.GetUsers(this.SearchModel.Email, this.SearchModel.Name, state.Page,
                state.PageSize);
            return new GridData<Entities.User>()
            {
                TotalItems = result.TotalCount,
                Items = result.Datum
            };
        };

        this.OnRemove = async (item) =>
        {
            var session = await this.GetUserSession();
            if (session.UserId == item.Id) return await Results<bool>.FailAsync("self delete not allowed");
            
            return await _userService.Remove(item.Id);
        };

        this.OnClick = async (id, obj) =>
        {
            var item = obj.xAs<Entities.User>();
            await this._userService.Lock(item.Id);
            item.LockoutEnabled = !item.LockoutEnabled;
        };
    }
}