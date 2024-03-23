using ClassicalApi.Blazor.Client.Models;

namespace ClassicalApi.Blazor.Client.Services;

public interface IComposerService
{
    Task<IEnumerable<ComposerModel>> GetAllAsync();
}
