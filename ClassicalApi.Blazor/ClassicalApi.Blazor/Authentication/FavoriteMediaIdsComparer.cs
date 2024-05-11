using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ClassicalApi.Blazor.Authentication;

public class FavoriteMediaIdsComparer : ValueComparer<HashSet<int>>
{
    public FavoriteMediaIdsComparer() : base(
        (l1, l2) => l1!.Order().SequenceEqual(l2!.Order()),
        l => l.Count == 0 ? l.Order().Aggregate((x, y) => x * 2 + y) : 0)
    {
    }
}
