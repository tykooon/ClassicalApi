using ClassicalApi.Core.Models;

namespace ClassicalApi.Core.Infrastructure;

public interface IComposerRepository
{
    //Qureies
    IEnumerable<Composer> GetAll();
    IEnumerable<Composer> Search(string query);
    Composer? GetById(int composerId);
    IEnumerable<Composer> GetByLastName(string composerId);
    byte[]? GetPortrait(int composerId);

    //Commands
    int AddNew(Composer newComposer);
    void Update(Composer composer);
    void Delete(int composerId);

    void AddPortrait(int composerId, byte[] image);
    void DeletePortrait(int composerId);
}
