using ClassicalApi.Blazor.Client.Models;

namespace ClassicalApi.Blazor.Client.Services;

public interface IComposerService
{
    Task<IEnumerable<ComposerModel>> Search(string name);
    Task<IEnumerable<ComposerModel>> GetComposers(IEnumerable<int>? ids = null);
    Task<ComposerModel?> GetById(int id);
    Task<int> AddNew(ComposerModel composer);
    Task<bool> Update(ComposerModel composer);
    Task<bool> DeleteById(int id);

    Task<string> GetPortrait(int composerId);
    Task<bool> AddPortrait(int composerId, string imageData);

    Task<IEnumerable<MediaLinkModel>> GetMediaLinks(int composerId);
    Task<IEnumerable<MediaLinkModel>> GetMediaLinksById(IEnumerable<int> linkIds);
    Task<bool> AddMedia(MediaLinkModel mediaLink);
    Task<bool> DeleteMediaById(int id);
}
