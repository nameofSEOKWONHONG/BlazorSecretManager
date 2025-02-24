using BlazorSecretManager.Infrastructure;
using BlazorSecretManager.Services.Auth;
using BlazorTrivialJs;
using eXtensionSharp;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MudBlazor;
using MudMvvMKit;
using MudMvvMKit.Base;
using MudMvvMKit.ViewComponents.ViewModels.ListView;
using SecretManager;

namespace BlazorSecretManager.Components.Pages.User.ViewModels;

public class UserListViewModel : MudListViewModel<Entities.User, UserSearchModel>, IUserListViewModel
{
    private readonly IUserService _userService;
    private readonly ITrivialJs _trivialJs;
    private readonly ISnackbar _snackbar;
    private readonly ProtectedSessionStorage _protectedSessionStorage;

    public UserListViewModel(MudUtility mudUtility,
        IUserService userService,
        ITrivialJs trivialJs,
        ISnackbar snackbar,
        ProtectedSessionStorage protectedSessionStorage) : base(mudUtility)
    {
        _userService = userService;
        _trivialJs = trivialJs;
        _snackbar = snackbar;
        _protectedSessionStorage = protectedSessionStorage;
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
            switch (id)
            {
                case "lock":
                {
                    await this._userService.Lock(item.Id);
                    item.LockoutEnabled = !item.LockoutEnabled;
                    break;
                }
                case "copyApiKey":
                {
                    await _trivialJs.CopyToClipboard(item.UserKey);
                    _snackbar.Add("secret key copied to clipboard", Severity.Success);
                    break;
                }
            }
        };
    }

    public override async Task InitializeAsync()
    {   
        this.UserSession = await _userService.GetUserSession();
    }

    public UserSession UserSession { get; set; }
}