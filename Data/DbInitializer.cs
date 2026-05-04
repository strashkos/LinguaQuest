using LinguaQuest.Web.Enums;
using LinguaQuest.Web.Models;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace LinguaQuest.Web.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(IServiceProvider services, IConfiguration _configuration)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<ApplicationUser>>();

        try
        {
            await context.UserLearningSettings.Indexes.CreateOneAsync(
                new CreateIndexModel<UserLearningSettings>(
                    Builders<UserLearningSettings>.IndexKeys.Ascending(item => item.UserId),
                    new CreateIndexOptions { Unique = true }));

            await context.DictionaryProgress.Indexes.CreateOneAsync(
                new CreateIndexModel<DictionaryProgress>(
                    Builders<DictionaryProgress>.IndexKeys
                        .Ascending(item => item.UserId)
                        .Ascending(item => item.WordPairId),
                    new CreateIndexOptions { Unique = true }));

            await context.WordPairs.Indexes.CreateOneAsync(
                new CreateIndexModel<WordPair>(
                    Builders<WordPair>.IndexKeys.Ascending(item => item.Id),
                    new CreateIndexOptions { Unique = true }));

            var demoUser = await context.Users.Find(item => item.Id == "demo").FirstOrDefaultAsync();
            if (demoUser is null)
            {
                demoUser = new ApplicationUser
                {
                    Id = "demo",
                    UserName = "demo",
                    DisplayName = "Demo learner",
                    Email = "demo@linguaquest.local",
                    CreatedUtc = DateTime.UtcNow
                };
                demoUser.PasswordHash = passwordHasher.HashPassword(demoUser, "Demo123!");
                await context.Users.InsertOneAsync(demoUser);
            }
            else if (string.IsNullOrWhiteSpace(demoUser.PasswordHash))
            {
                demoUser.PasswordHash = passwordHasher.HashPassword(demoUser, "Demo123!");
                demoUser.DisplayName = string.IsNullOrWhiteSpace(demoUser.DisplayName) ? "Demo learner" : demoUser.DisplayName;
                demoUser.Email = string.IsNullOrWhiteSpace(demoUser.Email) ? "demo@linguaquest.local" : demoUser.Email;

                await context.Users.ReplaceOneAsync(
                    item => item.Id == "demo",
                    demoUser,
                    new ReplaceOptions { IsUpsert = true });
            }

            var demoSettings = await context.UserLearningSettings
                .Find(item => item.UserId == "demo")
                .FirstOrDefaultAsync();

            if (demoSettings is null)
            {
                await context.UserLearningSettings.InsertOneAsync(new UserLearningSettings
                {
                    UserId = "demo",
                    SourceLanguage = LearningLanguage.Ukrainian,
                    TargetLanguage = LearningLanguage.English,
                    Level = LearningLevel.Level1,
                    WordsPerSession = 5,
                    ThemePreference = "dark"
                });
            }
            else
            {
                demoSettings.SourceLanguage = LearningLanguage.Ukrainian;
                demoSettings.TargetLanguage = LearningLanguage.English;
                demoSettings.Level = LearningLevel.Level1;
                demoSettings.WordsPerSession = 5;
                demoSettings.ThemePreference = string.IsNullOrWhiteSpace(demoSettings.ThemePreference) ? "dark" : demoSettings.ThemePreference;

                await context.UserLearningSettings.ReplaceOneAsync(
                    item => item.UserId == "demo",
                    demoSettings,
                    new ReplaceOptions { IsUpsert = true });
            }

            // Seed some demo progress so demo user appears to have learned a few words
            var existingProgress = await context.DictionaryProgress.Find(p => p.UserId == "demo").AnyAsync();
            if (!existingProgress)
            {
                var demoProgress = new List<DictionaryProgress>();
                // mark first 6 word pairs with some attempts
                for (int i = 1; i <= 6; i++)
                {
                    demoProgress.Add(new DictionaryProgress
                    {
                        UserId = "demo",
                        WordPairId = i,
                        Attempts = i + 2,
                        CorrectAttempts = Math.Max(1, i),
                        FirstSeenUtc = DateTime.UtcNow.AddDays(-10 + i),
                        LastSeenUtc = DateTime.UtcNow.AddDays(-1 + i)
                    });
                }
                if (demoProgress.Count > 0)
                {
                    await context.DictionaryProgress.InsertManyAsync(demoProgress);
                }
            }
        }
        catch (Exception)
        {
            // Let the app start even if MongoDB is unavailable in local development.
        }
    }

}
