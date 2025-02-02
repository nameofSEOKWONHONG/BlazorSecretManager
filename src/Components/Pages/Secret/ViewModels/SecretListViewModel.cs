using BlazorSecretManager.Components.Dialogs;
using BlazorSecretManager.Services.Secrets.Abstracts;
using eXtensionSharp;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudComposite.ViewComponents.Composites.ListView;

namespace BlazorSecretManager.Components.Pages.Secret.ViewModels;

public class SecretListViewModel : MudDataGridViewModel<Entities.Secret, SecretSearchModel>, ISecretListViewModel
{
    private readonly ISecretService _service;

    public SecretListViewModel(IDialogService dialogService, ISnackbar snackbar, NavigationManager navigationManager,
        ISecretService service) : base(dialogService, snackbar, navigationManager)
    {
        _service = service;
    }

    public override void Initialize()
    {
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
            var dlg = await this.DialogService.ShowAsync<SecretDialog>(title, parameters, options);
            var result = await dlg.Result;
            if (!result.Canceled)
            {
                return result.Data.xAs<Entities.Secret>();
            }

            return null;
        };        
        this.OnSave = async item => await _service.AddOrUpdate(item);
        this.OnSaveAfter = null;
    }
}