using Microsoft.JSInterop;

namespace BlazorTrivialJs;

public interface ITrivialJs
{
    Task<Dictionary<string, string>> GetBrowserInfo();
    Task Alert(string message);
    Task CopyToClipboard(string text);
    Task ScrollToBottom(string ele);
    Task GoBack();
    Task GoForward();
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
    
    public async Task ScrollToBottom(string ele)
    {
        await _jsRuntime.InvokeVoidAsync("scrollToBottom", ele);
    }

    public async Task GoBack()
    {
        await _jsRuntime.InvokeVoidAsync("window.history.back");
    }

    public async Task GoForward()
    {
        await _jsRuntime.InvokeVoidAsync("window.history.forward");
    }
}