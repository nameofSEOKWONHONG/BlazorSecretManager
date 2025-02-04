using System.ComponentModel;
using System.Globalization;

namespace BlazorSecretManager.Infrastructure;

public class AppState : INotifyPropertyChanged
{
    private string _currentCulture = CultureInfo.CurrentCulture.Name;

    public string CurrentCulture
    {
        get => _currentCulture;
        set
        {
            if (_currentCulture != value)
            {
                _currentCulture = value;
                CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(value);
                CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentCulture)));
            }
        }
    }

    public static Dictionary<string, CultureInfo> SupportedCultures = new()
    {
        { "English", new CultureInfo("en-US") },
        { "한국어", new CultureInfo("ko-KR") },
        { "Japan", new CultureInfo("ja-JP") },
    };
    public static Dictionary<string, string> SupportedLanguage = new()
    {
        { "en-US", "English" },
        { "ko-KR", "한국어" },
        { "ja-JP", "Japan" },
    };    

    public event PropertyChangedEventHandler PropertyChanged;
}