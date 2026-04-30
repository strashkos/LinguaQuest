using LinguaQuest.Web.Enums;
using LinguaQuest.Web.Models;
using LinguaQuest.Web.ViewModels;

namespace LinguaQuest.Web.Services.Interfaces;

public interface IWordService
{
    Task<IReadOnlyList<WordPair>> GetWordsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<WordPair>> GetWordsAsync(LearningLanguage sourceLanguage, LearningLanguage targetLanguage, LearningLevel level, CancellationToken cancellationToken = default);
    Task<WordPair?> GetRandomWordAsync(LearningLanguage sourceLanguage, LearningLanguage targetLanguage, LearningLevel level, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DictionaryItemViewModel>> GetDictionaryItemsAsync(string? search = null, CancellationToken cancellationToken = default);
}