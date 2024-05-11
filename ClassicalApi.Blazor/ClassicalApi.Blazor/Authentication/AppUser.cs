using Microsoft.AspNetCore.Identity;

namespace ClassicalApi.Blazor.Authentication;

public class AppUser : IdentityUser
{
    public HashSet<int> FavoriteMediaIds { get; set; } = [];
}
