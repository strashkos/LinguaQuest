using LinguaQuest.Web.Enums;
using LinguaQuest.Web.Models;
using LinguaQuest.Web.Services.Interfaces;
using LinguaQuest.Web.ViewModels;

namespace LinguaQuest.Web.Services.GameModes;

public class WordOnMindMode : GameModeBase
{
    public WordOnMindMode(IWordService wordService)
        : base(wordService)
    {
    }

    public override GameModeType Mode => GameModeType.WordOnMind;

    protected override string BuildPrompt(WordPair word)
    {
        return $"What does '{word.TargetText}' mean?";
    }

    protected override string BuildCorrectAnswer(WordPair word)
    {
        return word.SourceText;
    }

    protected override string BuildExplanation(WordPair word)
    {
        return $"{word.TargetText} means {word.SourceText}.";
    }

    protected override async Task<IReadOnlyList<string>> BuildOptionsAsync(
        LearningLanguage sourceLanguage,
        LearningLanguage targetLanguage,
        LearningLevel level,
        WordPair word,
        CancellationToken cancellationToken)
    {
        var words = await GetWordsAsync(sourceLanguage, targetLanguage, level, cancellationToken);
        return ShuffleAndTrimOptions(words.Select(item => item.SourceText), word.SourceText);
    }
}