namespace LinguaQuest.Web.Services.Interfaces;

public interface ICurrentUserService
{
    Task<string?> GetUserIdAsync(CancellationToken cancellationToken = default);
    Task<string?> GetDisplayNameAsync(CancellationToken cancellationToken = default);
}