using LinguaQuest.Web.Enums;
using LinguaQuest.Web.Models;
using LinguaQuest.Web.Services.Interfaces;

namespace LinguaQuest.Web.Services.GameModes;

public class SentenceComposeMode : GameModeBase
{
    public SentenceComposeMode(IWordService wordService)
        : base(wordService)
    {
    }

    public override GameModeType Mode => GameModeType.SentenceCompose;

    protected override string BuildPrompt(WordPair word)
    {
        return $"Впишіть речення за шаблоном: {SentenceTemplateBuilder.BuildPrompt(word)}";
    }

    protected override string BuildCorrectAnswer(WordPair word)
    {
        return SentenceTemplateBuilder.BuildAnswer(word);
    }

    protected override string BuildExplanation(WordPair word)
    {
        return $"Модельна відповідь: {SentenceTemplateBuilder.BuildAnswer(word)}";
    }

    protected override Task<IReadOnlyList<string>> BuildOptionsAsync(
        LearningLanguage sourceLanguage,
        LearningLanguage targetLanguage,
        LearningLevel level,
        WordPair word,
        CancellationToken cancellationToken)
    {
        return Task.FromResult<IReadOnlyList<string>>(Array.Empty<string>());
    }
}
