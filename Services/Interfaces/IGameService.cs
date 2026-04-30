using LinguaQuest.Web.Enums;
using LinguaQuest.Web.ViewModels;

namespace LinguaQuest.Web.Services.Interfaces;

public interface IGameService
{
    Task<GameTaskViewModel> CreateTaskAsync(LearningLanguage sourceLanguage, LearningLanguage targetLanguage, LearningLevel level, GameModeType? gameMode = null, CancellationToken cancellationToken = default);
    Task<GameResultViewModel> EvaluateAsync(GameTaskViewModel task, string selectedAnswer, CancellationToken cancellationToken = default);
}