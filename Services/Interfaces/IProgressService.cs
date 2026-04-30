using LinguaQuest.Web.Models;

namespace LinguaQuest.Web.Services.Interfaces;

public interface IProgressService
{
    Task<IReadOnlyList<DictionaryProgress>> GetProgressAsync(string userId, CancellationToken cancellationToken = default);
    Task<DictionaryProgress> RegisterAttemptAsync(string userId, int wordPairId, bool isCorrect, CancellationToken cancellationToken = default);
}