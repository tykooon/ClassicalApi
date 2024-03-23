using ClassicalApi.Web.Client.Models;
using ClassicalApi.Web.Client.Services;
using System.Text.Json;

namespace ClassicalApi.Web.Services;

public class ComposerService : IComposerService
{
    private readonly HttpClient _httpClient;
    private readonly static JsonSerializerOptions _jsonOptions = new JsonSerializerOptions()
    {
        PropertyNameCaseInsensitive = true
    };

    public ComposerService(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient();
        _httpClient.BaseAddress = new Uri("http://localhost:5000");
        _httpClient.DefaultRequestHeaders.Add("x-api-key", "muokY3cGjaA6juhmJmKyOOoZDhDscmrst2LosF9HieS5IH8o4JkkBroYEgqmn4yHVdXlqvpzm7Z5pn3iZqGJF5a8jL2SmcZzEHOEQpPeX1XermLkV6KImCybcDNQ3TVr");
    }

    public async Task<ComposerModel?> GetById(int id) => 
        await _httpClient.GetFromJsonAsync<ComposerModel>($"/composers/{id}");

    public async Task<IEnumerable<ComposerModel>> GetComposers() => 
        await _httpClient.GetFromJsonAsync<IEnumerable<ComposerModel>>("/composers") ?? [];
    
    public Task<IEnumerable<ComposerModel>> Search(string query)
    {
        throw new NotImplementedException();
    }

    public async Task<int> AddNew(ComposerModel composer)
    {
        var response = await _httpClient.PostAsJsonAsync("/composers", composer);
        if (response != null && response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            await Console.Out.WriteLineAsync(content);
            return JsonSerializer.Deserialize<int>(content, _jsonOptions);
        }
        return 0;
    }

    public async Task<bool> DeleteById(int id)
    {
        var response = await _httpClient.DeleteAsync($"/composers/{id}");
        return response != null && response.IsSuccessStatusCode;
    }

    public async Task<bool> Update(ComposerModel composer)
    {
        var response = await _httpClient.PutAsJsonAsync("/composers", composer);
        return response != null && response.IsSuccessStatusCode;
    }

    public async Task<string> GetPortrait(int composerId) => 
        await _httpClient.GetStringAsync($"/composers/{composerId}/portrait") ?? "";

    public async Task<bool> AddPortrait(int composerId, string imageData)
    {
        var response = await _httpClient.PostAsJsonAsync<PortraitData>($"/composers/{composerId}/portrait", new(imageData));
        return response != null && response.IsSuccessStatusCode;
    }
}
