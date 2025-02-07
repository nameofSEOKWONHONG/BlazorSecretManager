// using eXtensionSharp;
// using Microsoft.Extensions.Localization;
// using MudBlazor;
//
// namespace BlazorSecretManager.Infrastructure;
//
// public interface IJsonLocalizerInitializer
// {
//     Task InitializeAsync();
// }
//
// public class JsonLocalizer : MudLocalizer, IJsonLocalizerInitializer
// {
//     private List<LocalizeItem> _localization;
//
//     public JsonLocalizer()
//     {
//         
//     }
//
//
//     public async Task InitializeAsync()
//     {
//         var files = Directory.GetFiles("./wwwroot/localization/", "*.json");
//         foreach (var filePath in files)
//         {
//             var text = await File.ReadAllTextAsync(filePath);
//             var split = filePath.xSplit(".");
//             var map = text.xDeserialize<Dictionary<string, string>>();
//             LocalizeItem localizeItem = new LocalizeItem()
//             {
//                 Culture = split[0],
//                 Localization = map
//             };
//             _localization.Add(localizeItem);
//         }
//     }
//     
//     public override LocalizedString this[string key]
//     {
//         get
//         {
//             var currentCulture = Thread.CurrentThread.CurrentUICulture.Parent.Name;
//             var exists = _localization.FirstOrDefault(l => l.Culture == currentCulture);
//             if (exists.xIsEmpty())
//             {
//                 return 
//             }
//
//             return exists.Localization[key];
//             if (currentCulture.Equals("de", StringComparison.InvariantCultureIgnoreCase)
//                 && _localization.TryGetValue(key, out var res))
//             {
//                 return new(key, res);
//             }
//             else
//             {
//                 return new(key, key, true);
//             }
//         }
//     }
//     
// }
//
// class LocalizeItem
// {
//     public string Culture { get; set; }
//     public Dictionary<string, string> Localization { get; set; }
// }