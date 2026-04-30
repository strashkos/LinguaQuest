using LinguaQuest.Web.Enums;
using LinguaQuest.Web.Models;

namespace LinguaQuest.Web.Data.SeedData;

public static class WordPairsSeed
{
    public static IReadOnlyList<WordPair> Create() =>
    [
        new WordPair { Id = 1, SourceLanguage = LearningLanguage.Ukrainian, TargetLanguage = LearningLanguage.English, Level = LearningLevel.Level1, SourceText = "дім", TargetText = "house", Category = "home", Hint = "A place where people live" },
        new WordPair { Id = 2, SourceLanguage = LearningLanguage.Ukrainian, TargetLanguage = LearningLanguage.English, Level = LearningLevel.Level1, SourceText = "вода", TargetText = "water", Category = "basic", Hint = "Drink it every day" },
        new WordPair { Id = 3, SourceLanguage = LearningLanguage.Ukrainian, TargetLanguage = LearningLanguage.English, Level = LearningLevel.Level2, SourceText = "книга", TargetText = "book", Category = "school", Hint = "You read it" },
        new WordPair { Id = 4, SourceLanguage = LearningLanguage.Ukrainian, TargetLanguage = LearningLanguage.English, Level = LearningLevel.Level2, SourceText = "друг", TargetText = "friend", Category = "people", Hint = "A close person" },
        new WordPair { Id = 5, SourceLanguage = LearningLanguage.Ukrainian, TargetLanguage = LearningLanguage.German, Level = LearningLevel.Level1, SourceText = "дім", TargetText = "Haus", Category = "home", Hint = "A place where people live" },
        new WordPair { Id = 6, SourceLanguage = LearningLanguage.Ukrainian, TargetLanguage = LearningLanguage.German, Level = LearningLevel.Level1, SourceText = "вода", TargetText = "Wasser", Category = "basic", Hint = "Drink it every day" },
        new WordPair { Id = 7, SourceLanguage = LearningLanguage.Ukrainian, TargetLanguage = LearningLanguage.German, Level = LearningLevel.Level2, SourceText = "книга", TargetText = "Buch", Category = "school", Hint = "You read it" },
        new WordPair { Id = 8, SourceLanguage = LearningLanguage.Ukrainian, TargetLanguage = LearningLanguage.German, Level = LearningLevel.Level2, SourceText = "друг", TargetText = "Freund", Category = "people", Hint = "A close person" }
    ];
}