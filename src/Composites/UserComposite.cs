using BlazorSecretManager.Entities;
using BlazorSecretManager.Services.Auth;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudComposite;
using MudComposite.ViewComponents.Composites.ListView;

namespace BlazorSecretManager.Composites;

public class UserSearchModel
{
    public string Email { get; set; }
    public string Name { get; set; }
}

public interface IUserComposite : IMudDataGridComposite<User, UserSearchModel>
{
    
}

public class UserComposite : MudDataGridComposite<User, UserSearchModel>, IUserComposite
{
    private readonly IUserService _userService;

    public UserComposite(IDialogService dialogService, ISnackbar snackbar, NavigationManager navigationManager,
        IUserService userService) : base(dialogService, snackbar, navigationManager)
    {
        _userService = userService;
    }

    public override void Initialize()
    {
        this.OnServerReload = async (state) =>
        {
            var result = await _userService.GetUsers(this.SearchModel.Email, this.SearchModel.Name, state.Page,
                state.PageSize);
            return new GridData<User>()
            {
                TotalItems = result.TotalCount,
                Items = result.Datum
            };
        };

        this.OnRemove = async (item) =>
        {
            var session = await _userService.GetUserSession();
            if (session.UserId == item.Id) return await Results<bool>.FailAsync("self delete not allowed");
            
            return await _userService.Remove(item.Id);
        };
    }
}