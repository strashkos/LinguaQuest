using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using LinguaQuest.Web.Services.Interfaces;

namespace LinguaQuest.Web.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public CurrentUserService(AuthenticationStateProvider authenticationStateProvider)
    {
        _authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<string?> GetUserIdAsync(CancellationToken cancellationToken = default)
    {
        var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var user = state.User;
        return user.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public async Task<string?> GetDisplayNameAsync(CancellationToken cancellationToken = default)
    {
        var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var user = state.User;
        return user.Identity?.IsAuthenticated == true ? user.Identity.Name : null;
    }
}