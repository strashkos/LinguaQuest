using LinguaQuest.Web.Data;
using LinguaQuest.Web.Models;
using LinguaQuest.Web.Services.Interfaces;
using MongoDB.Driver;

namespace LinguaQuest.Web.Services;

public class UserLearningSettingsService : IUserLearningSettingsService
{
    private readonly IMongoCollection<UserLearningSettings> _settingsCollection;

    public UserLearningSettingsService(ApplicationDbContext db)
    {
        _settingsCollection = db.UserLearningSettings;
    }

    public async Task<UserLearningSettings> GetAsync(string userId, CancellationToken cancellationToken = default)
    {
        var settings = await _settingsCollection
            .Find(item => item.UserId == userId)
            .FirstOrDefaultAsync(cancellationToken);

        return settings ?? new UserLearningSettings { UserId = userId };
    }

    public async Task<UserLearningSettings> SaveAsync(UserLearningSettings settings, CancellationToken cancellationToken = default)
    {
        var existing = await _settingsCollection
            .Find(item => item.UserId == settings.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (existing is not null)
        {
            settings.Id = existing.Id;
        }

        await _settingsCollection.ReplaceOneAsync(
            item => item.UserId == settings.UserId,
            settings,
            new ReplaceOptions { IsUpsert = true },
            cancellationToken);

        return settings;
    }
}