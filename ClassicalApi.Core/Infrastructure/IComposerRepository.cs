using ClassicalApi.Core.Models;

namespace ClassicalApi.Core.Infrastructure;

public interface IComposerRepository
{
    IEnumerable<Composer> GetAll();
    Composer? GetById(int id);
    Composer? GetByLastName(string id);
}
