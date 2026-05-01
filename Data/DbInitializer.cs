using LinguaQuest.Web.Data.SeedData;
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

            var hasUkrainianSeed = await context.WordPairs
                .Find(item => item.SourceLanguage == LearningLanguage.Ukrainian)
                .AnyAsync();

            if (!hasUkrainianSeed)
            {
                await context.WordPairs.DeleteManyAsync(Builders<WordPair>.Filter.Empty);
                await context.WordPairs.InsertManyAsync(WordPairsSeed.Create());
            }

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
                    WordsPerSession = 5
                });
            }
            else
            {
                demoSettings.SourceLanguage = LearningLanguage.Ukrainian;
                demoSettings.TargetLanguage = LearningLanguage.English;
                demoSettings.Level = LearningLevel.Level1;
                demoSettings.WordsPerSession = 5;

                await context.UserLearningSettings.ReplaceOneAsync(
                    item => item.UserId == "demo",
                    demoSettings,
                    new ReplaceOptions { IsUpsert = true });
            }
        }
        catch (Exception)
        {
            // Let the app start even if MongoDB is unavailable in local development.
        }
    }
}