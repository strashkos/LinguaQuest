using LinguaQuest.Web.Models;

namespace LinguaQuest.Web.Data.SeedData;

public static class DefaultUsersSeed
{
    public static IReadOnlyList<ApplicationUser> Create() =>
    [
        new ApplicationUser { Id = "demo", UserName = "Demo learner", DisplayName = "Demo learner", Email = "demo@linguaquest.local" }
    ];
}