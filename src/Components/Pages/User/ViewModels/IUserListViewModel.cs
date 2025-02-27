﻿using BlazorSecretManager.Infrastructure;
using MudMvvMKit.ViewComponents.ViewModels.ListView;

namespace BlazorSecretManager.Components.Pages.User.ViewModels;

public interface IUserListViewModel : IMudListViewModel<Entities.User, UserSearchModel>
{
    UserSession UserSession { get; set; }
}