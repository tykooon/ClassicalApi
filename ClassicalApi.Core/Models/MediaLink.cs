using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ClassicalApi.Core.Models;

public class MediaLink
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;

    [JsonIgnore]
    public List<Composer> Composers { get; set; } = [];

    [NotMapped]
    public List<int> ComposerIds => Composers.Select(c  => c.Id).ToList();
}
