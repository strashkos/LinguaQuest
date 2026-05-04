using LinguaQuest.Web.Data;
using LinguaQuest.Web.Enums;
using LinguaQuest.Web.Models;
using LinguaQuest.Web.Services.Interfaces;
using LinguaQuest.Web.ViewModels;
using MongoDB.Driver;

namespace LinguaQuest.Web.Services;

public class WordService : IWordService
{
    private readonly IMongoCollection<WordPair> _wordPairs;

    public WordService(ApplicationDbContext db)
    {
        _wordPairs = db.WordPairs;
    }

    public async Task<IReadOnlyList<WordPair>> GetWordsAsync(CancellationToken cancellationToken = default)
    {
        return await TryGetMongoWordsAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<WordPair>> GetWordsAsync(LearningLanguage sourceLanguage, LearningLanguage targetLanguage, LearningLevel level, CancellationToken cancellationToken = default)
    {
        var allWords = await GetWordsAsync(cancellationToken);
        var pairWords = allWords
            .Where(item =>
                item.SourceLanguage == sourceLanguage &&
                item.TargetLanguage == targetLanguage)
            .OrderBy(item => item.Id)
            .ToList();

        return pairWords
            .Select(item => CloneWithLevel(item, GetEffectiveLevel(item)))
            .Where(item => item.Level == level)
            .ToList();
    }

    public async Task<WordPair?> GetRandomWordAsync(LearningLanguage sourceLanguage, LearningLanguage targetLanguage, LearningLevel level, CancellationToken cancellationToken = default)
    {
        var words = await GetWordsAsync(sourceLanguage, targetLanguage, level, cancellationToken);
        return words.Count == 0 ? null : words[Random.Shared.Next(words.Count)];
    }

    public async Task<IReadOnlyList<DictionaryItemViewModel>> GetDictionaryItemsAsync(string? search = null, CancellationToken cancellationToken = default)
    {
        var words = await GetWordsAsync(cancellationToken);
        var filtered = words.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            filtered = filtered.Where(item =>
                item.SourceText.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                item.TargetText.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                item.Category.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                item.Hint.Contains(term, StringComparison.OrdinalIgnoreCase));
        }

        return filtered
            .OrderBy(item => item.Id)
            .Select(item => new DictionaryItemViewModel
            {
                Id = item.Id,
                SourceText = item.SourceText,
                TargetText = item.TargetText,
                Category = item.Category,
                Hint = item.Hint,
                Level = GetEffectiveLevel(item)
            })
            .ToList();
    }

    private async Task<IReadOnlyList<WordPair>> TryGetMongoWordsAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await _wordPairs
                .Find(Builders<WordPair>.Filter.Empty)
                .SortBy(item => item.Id)
                .ToListAsync(cancellationToken);
        }
        catch
        {
            return Array.Empty<WordPair>();
        }
    }

    private static WordPair CloneWithLevel(WordPair word, LearningLevel level)
    {
        return new WordPair
        {
            Id = word.Id,
            SourceLanguage = word.SourceLanguage,
            TargetLanguage = word.TargetLanguage,
            Level = level,
            SourceText = word.SourceText,
            TargetText = word.TargetText,
            Category = word.Category,
            Hint = word.Hint
        };
    }

    private static LearningLevel GetEffectiveLevel(WordPair word)
    {
        var category = word.Category.Trim().ToLowerInvariant();
        var sourceText = word.SourceText.Trim().ToLowerInvariant();

        return (category, sourceText) switch
        {
            ("basic", _) => LearningLevel.Level1,
            ("home", _) => LearningLevel.Level2,
            ("animals", _) => LearningLevel.Level3,
            ("food", _) => LearningLevel.Level4,
            ("school", _) => LearningLevel.Level5,
            ("people", _) => LearningLevel.Level6,
            ("daily", _) => LearningLevel.Level7,
            ("city", _) => LearningLevel.Level8,
            ("travel", _) => LearningLevel.Level9,
            ("communication", _) => LearningLevel.Level10,
            ("work", _) => LearningLevel.Level11,
            ("science", _) => LearningLevel.Level12,
            ("nature", _) => LearningLevel.Level13,
            ("social", _) => LearningLevel.Level14,
            ("abstract", _) => LearningLevel.Level15,
            _ when sourceText is "дім" or "вода" or "їжа" => LearningLevel.Level1,
            _ when sourceText is "вокзал" or "квиток" or "дорога" => LearningLevel.Level9,
            _ when sourceText is "робота" or "команда" or "співпраця" => LearningLevel.Level11,
            _ => LearningLevel.Level8
        };
    }
}