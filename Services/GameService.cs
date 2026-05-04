using LinguaQuest.Web.Enums;
using LinguaQuest.Web.Services.Interfaces;
using LinguaQuest.Web.ViewModels;

namespace LinguaQuest.Web.Services;

public class GameService : IGameService
{
    private readonly IEnumerable<IGameMode> _modes;

    public GameService(IEnumerable<IGameMode> modes)
    {
        _modes = modes;
    }

    public Task<GameTaskViewModel> CreateTaskAsync(LearningLanguage sourceLanguage, LearningLanguage targetLanguage, LearningLevel level, GameModeType? gameMode = null, CancellationToken cancellationToken = default)
    {
        var modeList = _modes.ToList();
        var preferredMode = gameMode ?? level switch
        {
            LearningLevel.Level1 => GameModeType.SentenceChoice,
            LearningLevel.Level2 => GameModeType.SentenceCompose,
            _ => GameModeType.QuickFind
        };

        var mode = modeList.Count == 0
            ? throw new InvalidOperationException("No game modes are registered.")
            : modeList.FirstOrDefault(item => item.Mode == preferredMode) ?? modeList[0];
        return mode.CreateTaskAsync(sourceLanguage, targetLanguage, level, cancellationToken);
    }

    public Task<GameResultViewModel> EvaluateAsync(GameTaskViewModel task, string selectedAnswer, CancellationToken cancellationToken = default)
    {
        var isCorrect = task.Mode == GameModeType.SentenceCompose
            ? string.Equals(Normalize(selectedAnswer), Normalize(task.CorrectAnswer), StringComparison.Ordinal)
            : string.Equals(task.CorrectAnswer, selectedAnswer, StringComparison.OrdinalIgnoreCase);
        var message = isCorrect ? "Правильно." : $"Не зовсім. Правильна відповідь: {task.CorrectAnswer}.";

        return Task.FromResult(new GameResultViewModel
        {
            IsCorrect = isCorrect,
            Message = message,
            Explanation = task.Explanation,
            SelectedAnswer = selectedAnswer,
            CorrectAnswer = task.CorrectAnswer
        });
    }

    private static string Normalize(string input)
    {
        return string.Join(' ', input
            .Trim()
            .Trim('.', '!', '?', ',', ';', ':')
            .ToLowerInvariant()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries));
    }
}