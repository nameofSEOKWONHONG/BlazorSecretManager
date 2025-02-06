using System.Text;
using BlazorSecretManager.Components.Dialogs;
using BlazorSecretManager.Entities;
using BlazorSecretManager.Infrastructure;
using BlazorSecretManager.Services.Auth;
using BlazorSecretManager.Services.Secrets.Abstracts;
using BlazorTrivialJs;
using eXtensionSharp;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using MudComposite.Base;
using MudComposite.ViewComponents.Composites.ListView;

namespace BlazorSecretManager.Components.Pages.Secret.ViewModels;



public class SecretListViewModel : MudDataGridViewModel<Entities.Secret, SecretSearchModel>, ISecretListViewModel
{
    private readonly ISecretService _service;
    private readonly IUserService _userService;
    private readonly ITrivialJs _trivialJs;

    public SecretListViewModel(MudViewModelItem mudViewModelItem,
        ISecretService service,
        IUserService userService,
        ITrivialJs trivialJs) : base(mudViewModelItem)
    {
        _service = service;
        _userService = userService;
        _trivialJs = trivialJs;
    }

    public override void Initialize(MudDataGrid<Entities.Secret> dataGrid)
    {
        base.Initialize(dataGrid);
        
        this.OnServerReload = async state =>
        {
            var results  = await _service.GetSecrets(this.SearchModel.Title, this.SearchModel.Description, state.Page, state.PageSize);
            return new GridData<Entities.Secret>()
            {
                TotalItems = results.Item1,
                Items = results.Item2,
            };
        };
        this.OnRemove = async item => await _service.DeleteSecret(item.Id);
        this.OnSaveBefore = async item =>
        {
            var parameters = new DialogParameters()
            {
                { "Secret", item }
            };
            var options = new DialogOptions()
            {
                FullWidth = true,
                MaxWidth = MaxWidth.Medium
            };
            var title = string.Empty;
            if (item.Id > 0) title = "Modify";
            else title = "Create";
            var dlg = await this.Utility.DialogService.ShowAsync<SecretDialog>(title, parameters, options);
            var result = await dlg.Result;
            if (!result.Canceled)
            {
                return result.Data.xAs<Entities.Secret>();
            }
        
            return null;
        };        
        this.OnSave = async item => await _service.AddOrUpdate(item);
        this.OnSaveAfter = null;
        this.OnClick = async (id, obj) =>
        {
            if (id == "test")
            {
                if (this.SelectedItem.xIsEmpty())
                {
                    await this.Utility.DialogService.ShowMessageBox(id, "selected item is empty");    
                }
                else
                {
                    await this.Utility.DialogService.ShowMessageBox(id, $"item title: {this.SelectedItem.Title}");    
                }
            }
            else if (id == "getUrl")
            {
                var item = obj.xAs<Entities.Secret>();
                var url = await this._service.GetSecretUrl(item.Id);
                await _trivialJs.CopyToClipboard(url);
                this.Utility.Snackbar.Add("The URL has been copied", Severity.Success);
            }
            else if (id == "notice")
            {
                var userSession = await _userService.GetUserSession();
                using var client = new HttpClient();
                client.BaseAddress = new Uri(this.Utility.NavigationManager.BaseUri);
                var item = new Notification()
                {
                    UserId = userSession.UserId,
                    Type = "A",
                    Title = "New Secret",
                    Content = "New Secret",
                    Extra = "Notice",
                    PublishDate = DateTime.Now
                };
                var context = new StringContent(item.xSerialize(), Encoding.UTF8, "application/json");
                var res = await client.PostAsync("api/notice", context);
                res.EnsureSuccessStatusCode();
            }
        };
    }
}