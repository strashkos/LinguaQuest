using LinguaQuest.Web.Data;
using LinguaQuest.Web.Enums;
using LinguaQuest.Web.Models;
using LinguaQuest.Web.Services.Interfaces;
using LinguaQuest.Web.ViewModels;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.RegularExpressions;

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
        return await _wordPairs
            .Find(Builders<WordPair>.Filter.Empty)
            .SortBy(item => item.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<WordPair>> GetWordsAsync(LearningLanguage sourceLanguage, LearningLanguage targetLanguage, LearningLevel level, CancellationToken cancellationToken = default)
    {
        var filter = Builders<WordPair>.Filter.Where(item =>
            item.SourceLanguage == sourceLanguage &&
            item.TargetLanguage == targetLanguage &&
            item.Level == level);

        return await _wordPairs
            .Find(filter)
            .SortBy(item => item.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<WordPair?> GetRandomWordAsync(LearningLanguage sourceLanguage, LearningLanguage targetLanguage, LearningLevel level, CancellationToken cancellationToken = default)
    {
        var words = await GetWordsAsync(sourceLanguage, targetLanguage, level, cancellationToken);
        return words.Count == 0 ? null : words[Random.Shared.Next(words.Count)];
    }

    public async Task<IReadOnlyList<DictionaryItemViewModel>> GetDictionaryItemsAsync(string? search = null, CancellationToken cancellationToken = default)
    {
        var filter = Builders<WordPair>.Filter.Empty;

        if (!string.IsNullOrWhiteSpace(search))
        {
            var escaped = Regex.Escape(search.Trim());
            var regex = new BsonRegularExpression(escaped, "i");
            filter = Builders<WordPair>.Filter.Or(
                Builders<WordPair>.Filter.Regex(item => item.SourceText, regex),
                Builders<WordPair>.Filter.Regex(item => item.TargetText, regex),
                Builders<WordPair>.Filter.Regex(item => item.Category, regex),
                Builders<WordPair>.Filter.Regex(item => item.Hint, regex));
        }

        var words = await _wordPairs
            .Find(filter)
            .SortBy(item => item.Id)
            .ToListAsync(cancellationToken);

        return words
            .Select(item => new DictionaryItemViewModel
            {
                Id = item.Id,
                SourceText = item.SourceText,
                TargetText = item.TargetText,
                Category = item.Category,
                Hint = item.Hint,
                Level = item.Level
            })
            .ToList();
    }
}