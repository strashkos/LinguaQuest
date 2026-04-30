using LinguaQuest.Web.Enums;
using LinguaQuest.Web.Models;

namespace LinguaQuest.Web.ViewModels;

public class LearningSettingsViewModel
{
    public LearningLanguage SourceLanguage { get; set; } = LearningLanguage.Ukrainian;
    public LearningLanguage TargetLanguage { get; set; } = LearningLanguage.English;
    public LearningLevel Level { get; set; } = LearningLevel.Level1;
    public int WordsPerSession { get; set; } = 5;

    public static LearningSettingsViewModel FromModel(UserLearningSettings model) => new()
    {
        SourceLanguage = model.SourceLanguage,
        TargetLanguage = model.TargetLanguage,
        Level = model.Level,
        WordsPerSession = model.WordsPerSession
    };

    public UserLearningSettings ToModel(string userId) => new()
    {
        UserId = userId,
        SourceLanguage = SourceLanguage,
        TargetLanguage = TargetLanguage,
        Level = Level,
        WordsPerSession = WordsPerSession
    };
}