namespace BlazorSecretManager.Services.Menu.Abstracts;

public interface IMenuService
{
    Task<List<Entities.Menu>> GetMenuWithSubMenusAsync();

}