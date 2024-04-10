namespace ClassicalApi.Blazor.Client.Models;

public class MediaLinkModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public List<int> ComposerIds { get; set; } = [];
}
