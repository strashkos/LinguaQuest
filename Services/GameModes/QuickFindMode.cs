using LinguaQuest.Web.Enums;
using LinguaQuest.Web.Models;
using LinguaQuest.Web.Services.Interfaces;
using LinguaQuest.Web.ViewModels;

namespace LinguaQuest.Web.Services.GameModes;

public class QuickFindMode : GameModeBase
{
    public QuickFindMode(IWordService wordService)
        : base(wordService)
    {
    }

    public override GameModeType Mode => GameModeType.QuickFind;

    protected override string BuildPrompt(WordPair word)
    {
        return $"Choose the translation for '{word.SourceText}'";
    }

    protected override string BuildExplanation(WordPair word)
    {
        return $"{word.SourceText} means {word.TargetText}.";
    }

    protected override async Task<IReadOnlyList<string>> BuildOptionsAsync(
        LearningLanguage sourceLanguage,
        LearningLanguage targetLanguage,
        LearningLevel level,
        WordPair word,
        CancellationToken cancellationToken)
    {
        var words = await GetWordsAsync(sourceLanguage, targetLanguage, level, cancellationToken);
        return ShuffleAndTrimOptions(words.Select(item => item.TargetText), word.TargetText);
    }
}
