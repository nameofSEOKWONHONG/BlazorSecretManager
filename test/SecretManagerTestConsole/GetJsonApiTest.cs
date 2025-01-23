using System.Net.Http.Json;
using System.Text.Json;

namespace SecretManagerTestConsole;

public class GetJsonApiTest
{
    public async Task GetJsonApi()
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri("https://localhost:7283");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Add("Authorization", "7977ab146823436dadc256529bc6d143");
        var res = await client.GetAsync("api/secret/cfa4262be0364df2811d260887695f46/1");
        res.EnsureSuccessStatusCode();
        var result = await res.Content.ReadFromJsonAsync<Root>();
        Console.WriteLine(JsonSerializer.Serialize(result));
    }
}

class Root
{
    public string A { get; set; }
}