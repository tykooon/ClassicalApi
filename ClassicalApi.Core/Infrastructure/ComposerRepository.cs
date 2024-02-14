using ClassicalApi.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassicalApi.Core.Infrastructure;

public class ComposerRepository : IComposerRepository
{
    private readonly DbSet<Composer> _composers;

    public ComposerRepository(AppDbContext context)
    {
        _composers = context.Composers;
    }

    public IEnumerable<Composer> GetAll() => _composers.ToList();

    public Composer? GetById(int id) => _composers.Find(id);

    public Composer? GetByLastName(string lastName) => _composers.FirstOrDefault(x => x.LastName.ToLower() == lastName.ToLower());
}
