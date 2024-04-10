namespace ClassicalApi.Host.Models;

public class AddMediaLinkRequest
{
    public string Title { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public List<int> ComposerIds { get; set; } = [];
}
