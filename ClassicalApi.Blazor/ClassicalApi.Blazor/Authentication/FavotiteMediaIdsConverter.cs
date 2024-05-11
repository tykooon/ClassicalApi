using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace ClassicalApi.Blazor.Authentication;

public class FavotiteMediaIdsConverter : ValueConverter<HashSet<int>, string>
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.Strict
    };

    public FavotiteMediaIdsConverter() : base(
                f => JsonSerializer.Serialize(f, _jsonOptions),
                f => JsonSerializer.Deserialize<HashSet<int>>(f, _jsonOptions) ?? new HashSet<int>())
    { }
}
