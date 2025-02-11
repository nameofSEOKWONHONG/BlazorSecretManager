using BlazorSecretManager.Infrastructure;
using BlazorSecretManager.Services.Auth;
using eXtensionSharp;
using MudBlazor;
using MudMvvMKit;
using MudMvvMKit.Base;
using MudMvvMKit.ViewComponents.ViewModels.ListView;

namespace BlazorSecretManager.Components.Pages.User.ViewModels;

public class UserListViewModel : MudListViewModel<Entities.User, UserSearchModel>, IUserListViewModel
{
    private readonly IUserService _userService;
    
    public UserListViewModel(MudUtility mudUtility,
        IUserService userService) : base(mudUtility)
    {
        _userService = userService;
    }

    public override void Initialize()
    {
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

    public override async Task InitializeAsync()
    {   
        this.UserSession = await _userService.GetUserSession();
    }

    public UserSession UserSession { get; set; }
}