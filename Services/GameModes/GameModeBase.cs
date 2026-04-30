using LinguaQuest.Web.Enums;
using LinguaQuest.Web.Models;
using LinguaQuest.Web.Services.Interfaces;
using LinguaQuest.Web.ViewModels;

namespace LinguaQuest.Web.Services.GameModes;

public abstract class GameModeBase : IGameMode
{
    private readonly IWordService _wordService;

    protected GameModeBase(IWordService wordService)
    {
        _wordService = wordService;
    }

    public abstract GameModeType Mode { get; }

    public async Task<GameTaskViewModel> CreateTaskAsync(
        LearningLanguage sourceLanguage,
        LearningLanguage targetLanguage,
        LearningLevel level,
        CancellationToken cancellationToken = default)
    {
        var word = await _wordService.GetRandomWordAsync(sourceLanguage, targetLanguage, level, cancellationToken)
            ?? throw new InvalidOperationException("No words are available for the selected settings.");

        var options = await BuildOptionsAsync(sourceLanguage, targetLanguage, level, word, cancellationToken);

        return new GameTaskViewModel
        {
            Mode = Mode,
            SourceLanguage = sourceLanguage,
            TargetLanguage = targetLanguage,
            Level = level,
            WordPairId = word.Id,
            SourceText = word.SourceText,
            TargetText = word.TargetText,
            Prompt = BuildPrompt(word),
            CorrectAnswer = BuildCorrectAnswer(word),
            Options = options,
            Hint = word.Hint,
            Explanation = BuildExplanation(word)
        };
    }

    protected abstract string BuildPrompt(WordPair word);

    protected virtual string BuildCorrectAnswer(WordPair word)
    {
        return word.TargetText;
    }

    protected abstract string BuildExplanation(WordPair word);

    protected abstract Task<IReadOnlyList<string>> BuildOptionsAsync(
        LearningLanguage sourceLanguage,
        LearningLanguage targetLanguage,
        LearningLevel level,
        WordPair word,
        CancellationToken cancellationToken);

    protected static IReadOnlyList<string> ShuffleAndTrimOptions(IEnumerable<string> items, string requiredAnswer, int takeCount = 4)
    {
        var options = items
            .Where(item => !string.IsNullOrWhiteSpace(item))
            .Distinct()
            .ToList();

        if (!options.Contains(requiredAnswer))
        {
            options.Insert(0, requiredAnswer);
        }

        return options
            .Distinct()
            .OrderBy(_ => Random.Shared.Next())
            .Take(takeCount)
            .ToList();
    }

    protected async Task<IReadOnlyList<WordPair>> GetWordsAsync(
        LearningLanguage sourceLanguage,
        LearningLanguage targetLanguage,
        LearningLevel level,
        CancellationToken cancellationToken)
    {
        return await _wordService.GetWordsAsync(sourceLanguage, targetLanguage, level, cancellationToken);
    }
}