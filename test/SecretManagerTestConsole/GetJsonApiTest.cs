using System.Text.Json;

namespace SecretManagerTestConsole;

public class GetJsonApiTest
{
    public async Task GetJsonApi()
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri("https://localhost:7283");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Add("apiKey", "6129409c822c4c97807fa36d741a8f64");
        var res = await client.GetAsync("api/secret/80a1a95bac0b4299a1c3f798907b0958/1");
        res.EnsureSuccessStatusCode();
        var result = await res.Content.ReadAsStringAsync();
        Console.WriteLine(JsonSerializer.Serialize(result));
    }
}

class Root
{
    public string A { get; set; }
}