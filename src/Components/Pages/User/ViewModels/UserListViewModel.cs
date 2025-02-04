using BlazorSecretManager.Infrastructure;
using BlazorSecretManager.Services.Auth;
using eXtensionSharp;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using MudComposite;
using MudComposite.Base;
using MudComposite.ViewComponents.Composites.ListView;

namespace BlazorSecretManager.Components.Pages.User.ViewModels;

public class UserListViewModel : MudDataGridViewModel<Entities.User, UserSearchModel>, IUserListViewModel
{
    private readonly IUserService _userService;
    
    public UserListViewModel(MudViewModelItem mudViewModelItem,
        IUserService userService) : base(mudViewModelItem)
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
            var session = await this._userService.GetUserSession();
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

    public override async Task InitializeAsync(MudDataGrid<Entities.User> dataGrid)
    {
        await base.InitializeAsync(dataGrid);
        
        this.UserSession = await _userService.GetUserSession();
    }

    public UserSession UserSession { get; set; }
}