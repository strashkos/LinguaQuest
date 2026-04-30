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
        var mode = modeList.Count == 0
            ? throw new InvalidOperationException("No game modes are registered.")
            : gameMode is null
                ? modeList[Random.Shared.Next(modeList.Count)]
                : modeList.FirstOrDefault(item => item.Mode == gameMode.Value) ?? modeList[0];
        return mode.CreateTaskAsync(sourceLanguage, targetLanguage, level, cancellationToken);
    }

    public Task<GameResultViewModel> EvaluateAsync(GameTaskViewModel task, string selectedAnswer, CancellationToken cancellationToken = default)
    {
        var isCorrect = string.Equals(task.CorrectAnswer, selectedAnswer, StringComparison.OrdinalIgnoreCase);
        var message = isCorrect ? "Correct answer." : $"Not quite. The right answer is {task.CorrectAnswer}.";

        return Task.FromResult(new GameResultViewModel
        {
            IsCorrect = isCorrect,
            Message = message,
            Explanation = task.Explanation,
            SelectedAnswer = selectedAnswer,
            CorrectAnswer = task.CorrectAnswer
        });
    }
}