using LinguaQuest.Web.ViewModels;

namespace LinguaQuest.Web.Services.Interfaces;

public interface IAuthService
{
    Task<(bool Success, string? Error)> RegisterAsync(RegisterViewModel model, HttpContext httpContext, CancellationToken cancellationToken = default);
    Task<(bool Success, string? Error)> LoginAsync(LoginViewModel model, HttpContext httpContext, CancellationToken cancellationToken = default);
    Task LogoutAsync(HttpContext httpContext, CancellationToken cancellationToken = default);
}