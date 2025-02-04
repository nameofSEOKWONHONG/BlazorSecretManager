using Microsoft.JSInterop;

namespace BlazorTrivialJs;

public interface ITrivialJs
{
    Task<Dictionary<string, string>> GetBrowserInfo();
    Task Alert(string message);
    Task CopyToClipboard(string text);
}

public class TrivialJs : ITrivialJs
{
    private readonly IJSRuntime _jsRuntime;

    public TrivialJs(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }
    
    public async Task<Dictionary<string, string>> GetBrowserInfo()
    {
        return await _jsRuntime.InvokeAsync<Dictionary<string, string>>("getBrowserInfo", _jsRuntime);
    }

    public async Task Alert(string message)
    {
        await _jsRuntime.InvokeVoidAsync("window.alert", message);
    }

    public async Task CopyToClipboard(string text)
    {
        await _jsRuntime.InvokeVoidAsync("copyToClipboard", text);
    }    
}