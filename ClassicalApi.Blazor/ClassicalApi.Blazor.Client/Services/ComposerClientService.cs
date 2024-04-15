using ClassicalApi.Blazor.Client.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace ClassicalApi.Blazor.Client.Services;

public class ComposerClientService : IComposerService
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _httpClient;

    public ComposerClientService(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient("ApiServer");
    }

    public async Task<ComposerModel?> GetById(int id) =>
        await _httpClient.GetFromJsonAsync<ComposerModel>($"/composers/{id}");

    public async Task<IEnumerable<ComposerModel>> GetComposers() =>
        await _httpClient.GetFromJsonAsync<IEnumerable<ComposerModel>>("/composers") ?? [];

    public async Task<IEnumerable<ComposerModel>> Search(string name) =>
        await _httpClient.GetFromJsonAsync<IEnumerable<ComposerModel>>($"/composers/search?query={name}") ?? [];

    public async Task<int> AddNew(ComposerModel composer)
    {
        var response = await _httpClient.PostAsJsonAsync<ComposerModel>("/composers", composer);
        if (response != null)
        {
            var content = await response.Content.ReadAsStringAsync();
            await Console.Out.WriteLineAsync("Interactive Client" + content);
            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<int>(content, _jsonOptions);
            }
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
        var response = await _httpClient.PutAsJsonAsync<ComposerModel>("/composers", composer);
        return response != null && response.IsSuccessStatusCode;
    }
        

    public async Task<string> GetPortrait(int composerId) => 
        await _httpClient.GetStringAsync($"/composers/{composerId}/portrait") ?? "";

    public async Task<bool> AddPortrait(int composerId, string imageData)
    {
        var response = await _httpClient.PostAsJsonAsync<PortraitData>($"/composers/{composerId}/portrait", new(imageData) );
        if (response != null)
        {
            var content = response.Content.ReadAsStringAsync();
            await Console.Out.WriteLineAsync("Ineractive Client: " + content); // TODO: Logging
        }
        return response != null && response.IsSuccessStatusCode;
    }

    public async Task<IEnumerable<MediaLinkModel>> GetMediaLinks(int id) =>
        await _httpClient.GetFromJsonAsync<IEnumerable<MediaLinkModel>>($"/medialinks?composerId={id}") ?? [];

    public async Task<bool> AddMedia(MediaLinkModel mediaLink)
    {
        var response = await _httpClient.PostAsJsonAsync<MediaLinkModel>($"/medialinks/", mediaLink);
        return response != null && response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteMediaById(int id)
    {
        var response = await _httpClient.DeleteAsync($"/medialinks/{id}");
        return response != null && response.IsSuccessStatusCode;
    }
}
