using LinguaQuest.Web.Enums;
using LinguaQuest.Web.ViewModels;

namespace LinguaQuest.Web.Services.Interfaces;

public interface IGameMode
{
    GameModeType Mode { get; }
    Task<GameTaskViewModel> CreateTaskAsync(LearningLanguage sourceLanguage, LearningLanguage targetLanguage, LearningLevel level, CancellationToken cancellationToken = default);
}