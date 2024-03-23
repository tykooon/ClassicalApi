using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ClassicalApi.Core.Models;

public class Portrait
{
    public int Id { get; set; }

    [JsonIgnore]
    public Composer Composer { get; set; } = new();

    public byte[] Image { get; set; } = [];
}
