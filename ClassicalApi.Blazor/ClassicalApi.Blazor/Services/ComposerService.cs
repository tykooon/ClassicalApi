using ClassicalApi.Blazor.Client.Models;
using ClassicalApi.Blazor.Client.Services;
using System.Text;
using System.Text.Json;

namespace ClassicalApi.Blazor.Services;

public class ComposerService : IComposerService
{
    private readonly HttpClient _httpClient;
    private readonly static JsonSerializerOptions _jsonOptions = new JsonSerializerOptions()
    {
        PropertyNameCaseInsensitive = true
    };

    public ComposerService(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient("ApiServer");
    }

    public async Task<ComposerModel?> GetById(int id) => 
        await _httpClient.GetFromJsonAsync<ComposerModel>($"/composers/{id}");

    public async Task<IEnumerable<ComposerModel>> GetComposers(IEnumerable<int>? ids = null)
    {
        IEnumerable<ComposerModel> result = [];
        if(ids == null)
        {
            result = await _httpClient.GetFromJsonAsync<IEnumerable<ComposerModel>>("/composers") ?? [];
        }
        else
        {
            var requestUri = new StringBuilder("/composers?");
            foreach (var id in ids)
            {
                requestUri.Append($"id={id}&");
            }
            var request = requestUri.ToString()[..^1];
            result = await _httpClient.GetFromJsonAsync<IEnumerable<ComposerModel>>(request) ?? [];
        }
        return result;
    }

    public async Task<IEnumerable<ComposerModel>> Search(string name) =>
        await _httpClient.GetFromJsonAsync<IEnumerable<ComposerModel>>($"/composers/search?query={name}") ?? [];

    public async Task<int> AddNew(ComposerModel composer)
    {
        var response = await _httpClient.PostAsJsonAsync("/composers", composer);
        if (response != null && response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            await Console.Out.WriteLineAsync("Interactive Server:" + content);
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
        var content = await response.Content.ReadAsStringAsync();
        await Console.Out.WriteLineAsync("Interactive Server:" + content); // TODO: Logging
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

    public async Task<IEnumerable<MediaLinkModel>> GetMediaLinksById(IEnumerable<int> linkIds)
    {
        var requestUri = new StringBuilder("/medialinks/selected?");
        foreach (var linkId in linkIds)
        {
            requestUri.Append($"id={linkId}&");
        }
        var request = requestUri.ToString()[..^1];
        return await _httpClient.GetFromJsonAsync<IEnumerable<MediaLinkModel>>(request) ?? [];
    }
}
