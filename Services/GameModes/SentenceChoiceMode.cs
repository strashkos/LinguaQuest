using LinguaQuest.Web.Enums;
using LinguaQuest.Web.Models;
using LinguaQuest.Web.Services.Interfaces;

namespace LinguaQuest.Web.Services.GameModes;

public class SentenceChoiceMode : GameModeBase
{
    public SentenceChoiceMode(IWordService wordService)
        : base(wordService)
    {
    }

    public override GameModeType Mode => GameModeType.SentenceChoice;

    protected override string BuildPrompt(WordPair word)
    {
        return $"Оберіть слово для речення: {SentenceTemplateBuilder.BuildPrompt(word)}";
    }

    protected override string BuildExplanation(WordPair word)
    {
        return $"Слово '{word.TargetText}' підходить до цього речення.";
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
