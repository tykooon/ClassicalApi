using ClassicalApi.Web.Client.Models;

namespace ClassicalApi.Web.Client.Services;

public interface IComposerService
{
    Task<IEnumerable<ComposerModel>> Search(string name);
    Task<IEnumerable<ComposerModel>> GetComposers();
    Task<ComposerModel?> GetById(int id);
    Task<int> AddNew(ComposerModel composer);
    Task<bool> Update(ComposerModel composer);
    Task<bool> DeleteById(int id);

    Task<string> GetPortrait(int composerId);
    Task<bool> AddPortrait(int composerId, string imageData);
}
