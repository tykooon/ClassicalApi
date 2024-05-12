using ClassicalApi.Blazor.Authentication;

namespace ClassicalApi.Blazor.Services;

public interface ICurrentUserService
{
    Task<AppUser?> GetUserInfoAsync();
    Task<bool> IsAdmin();
    Task<bool> IsAuthenticated();
    Task<bool> IsSuperAdmin();
    Task<HashSet<int>> AddMediaToUserFavorites(int mediaId);
    Task<HashSet<int>> DeleteMediaFromUserFavorites(int mediaId);
}
