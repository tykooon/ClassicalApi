using ClassicalApi.Blazor.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ClassicalApi.Blazor.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public CurrentUserService(UserManager<AppUser> userManager, AuthenticationStateProvider authenticationStateProvider)
    {
        _userManager = userManager;
        _authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<AppUser?> GetUserInfoAsync()
    {
        var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var result = await _userManager.FindByNameAsync(state.User.FindFirst(ClaimTypes.Name)?.Value ?? "");
        return result;
    }

    public async Task<bool> IsAdmin()
    {
        var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
        return state != null && (state.User.IsInRole("Administrator") || state.User.IsInRole("SuperAdmin"));
    }

    public async Task<bool> IsAuthenticated()
    {
        var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
        return state?.User?.Identity != null && state.User.Identity.IsAuthenticated;
    }

    public async Task<bool> IsSuperAdmin()
    {
        var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
        return state != null && state.User.IsInRole("SuperAdmin");
    }

    public async Task<HashSet<int>> AddMediaToUserFavorites(int mediaId)
    {
        var user = await GetUserInfoAsync();
        if (user == null)
        {
            return [];
        }
        user.FavoriteMediaIds.Add(mediaId);
        await _userManager.UpdateAsync(user);
        return user.FavoriteMediaIds;
    }

    public async Task<HashSet<int>> DeleteMediaFromUserFavorites(int mediaId)
    {
        var user = await GetUserInfoAsync();
        if (user == null)
        {
            return [];
        }
        user.FavoriteMediaIds.Remove(mediaId);
        await _userManager.UpdateAsync(user);
        return user.FavoriteMediaIds;
    }
}
