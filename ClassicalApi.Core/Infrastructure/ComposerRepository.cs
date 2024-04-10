using ClassicalApi.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassicalApi.Core.Infrastructure;

public class ComposerRepository : IComposerRepository
{
    private readonly DbSet<Composer> _composers;
    private readonly DbSet<Portrait> _portraits;
    private readonly DbSet<MediaLink> _mediaLinks;
    private readonly AppDbContext _ctx;

    public ComposerRepository(AppDbContext context)
    {
        _composers = context.Composers;
        _portraits = context.Portraits;
        _mediaLinks = context.MediaLinks;
        _ctx = context;
    }

    public IEnumerable<Composer> GetAll() =>
        _composers.ToList();

    public Composer? GetById(int id) =>
        _composers.Find(id);

    public IEnumerable<Composer> GetByLastName(string lastName) =>
        _composers.Where(x => x.LastName.Equals(lastName, StringComparison.CurrentCultureIgnoreCase)).ToList();

    public int AddNew(Composer composer)
    {
        _composers.Add(composer);
        _ctx.SaveChanges();
        return composer.Id;
    }

    public void Update(Composer composer)
    {
        var oldComposer = _composers.Find(composer.Id);
        if (oldComposer != null)
        {
            oldComposer.ShortBio = composer.ShortBio;
            oldComposer.YearOfBirth = composer.YearOfBirth;
            oldComposer.YearOfDeath = composer.YearOfDeath;
            oldComposer.CityOfBirth = composer.CityOfBirth;
            oldComposer.CountryOfBirth = composer.CountryOfBirth;
            oldComposer.FirstName = composer.FirstName;
            _ctx.SaveChanges();
        }
    }

    public void Delete(int composerId)
    {
        var oldComposer = _composers.Find(composerId);
        if(oldComposer != null)
        {
            _composers.Remove(oldComposer);
            _ctx.SaveChanges();
        }
    }

    public byte[]? GetPortrait(int id)
    {
        var composer = _composers.Include(c => c.Portrait).Where(c => c.Id == id).SingleOrDefault();
        return composer?.Portrait?.Image;
    }

    public void AddPortrait(int composerId, byte[] image)
    {
        var composer = _composers.Find(composerId);
        if (composer == null)
        {
            return;
        }

        var portrait = _portraits.Where(p => p.Id == composer.PortraitId).SingleOrDefault();
        if (portrait == null)
        {
            var p = new Portrait() { Image = image };
            composer.Portrait = p;
        }
        else
        {
            portrait.Image = image;
        }
        _ctx.SaveChanges();
    }

    public void DeletePortrait(int composerId)
    {
        var composer = _composers.Include(c => c.Portrait).Where(c => c.Id == composerId).SingleOrDefault();
        if (composer != null && composer.Portrait != null)
        {
            _portraits.Remove(composer.Portrait);
            _ctx.SaveChanges();
        }
    }

    public IEnumerable<Composer> Search(string query)
    {
        var result =  _composers.Where(c =>
            EF.Functions.Like(c.FirstName, $"%{query}%") ||
            EF.Functions.Like(c.LastName, $"%{query}%") ||
            EF.Functions.Like(c.CityOfBirth, $"%{query}%") ||
            EF.Functions.Like(c.CountryOfBirth, $"%{query}%")).ToList();
        return result;
    }

    public IEnumerable<MediaLink> GetMediaLinks(int composerId) => 
        _mediaLinks.Where(m => m.Composers.Select(c => c.Id).Contains(composerId)).ToList();

    public MediaLink? GetMediaLinkById(int mediaId) =>
        _mediaLinks.Find(mediaId);



    public int AddNewMedia(MediaLink newMedia)
    {
        if (newMedia == null || newMedia.Composers.Count == 0)
        {
            return 0;
        }

        foreach (var composerId in newMedia.ComposerIds)
        {
            var composer = _composers.Find(composerId);
            if (composer == null)
            {
                return 0;
            }
        }

        _mediaLinks.Add(newMedia);
        _ctx.SaveChanges();
        return newMedia.Id;
    }

    public void DeleteMedia(int mediaId)
    {
        var media = _mediaLinks.Find(mediaId);
        if (media != null)
        {
            _mediaLinks.Remove(media);
            _ctx.SaveChanges();
        }
    }
}
