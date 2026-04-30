using LinguaQuest.Web.Models;

namespace LinguaQuest.Web.Services.Interfaces;

public interface IUserLearningSettingsService
{
    Task<UserLearningSettings> GetAsync(string userId, CancellationToken cancellationToken = default);
    Task<UserLearningSettings> SaveAsync(UserLearningSettings settings, CancellationToken cancellationToken = default);
}