using LinguaQuest.Web.Data;
using LinguaQuest.Web.Models;
using LinguaQuest.Web.Services.Interfaces;
using MongoDB.Driver;

namespace LinguaQuest.Web.Services;

public class ProgressService : IProgressService
{
    private readonly IMongoCollection<DictionaryProgress> _progressCollection;
    private readonly IMongoCollection<WordPair> _wordPairCollection;

    public ProgressService(ApplicationDbContext db)
    {
        _progressCollection = db.DictionaryProgress;
        _wordPairCollection = db.WordPairs;
    }

    public async Task<IReadOnlyList<DictionaryProgress>> GetProgressAsync(string userId, CancellationToken cancellationToken = default)
    {
        var progress = await _progressCollection
            .Find(item => item.UserId == userId)
            .SortByDescending(item => item.LastSeenUtc)
            .ToListAsync(cancellationToken);

        if (progress.Count == 0)
        {
            return progress;
        }

        var wordIds = progress.Select(item => item.WordPairId).Distinct().ToList();
        var words = await _wordPairCollection
            .Find(item => wordIds.Contains(item.Id))
            .ToListAsync(cancellationToken);

        var byId = words.ToDictionary(item => item.Id);
        foreach (var item in progress)
        {
            if (byId.TryGetValue(item.WordPairId, out var wordPair))
            {
                item.WordPair = wordPair;
            }
        }

        return progress;
    }

    public async Task<DictionaryProgress> RegisterAttemptAsync(string userId, int wordPairId, bool isCorrect, CancellationToken cancellationToken = default)
    {
        var progress = await _progressCollection.Find(item => item.UserId == userId && item.WordPairId == wordPairId)
            .FirstOrDefaultAsync(cancellationToken);

        if (progress is null)
        {
            progress = new DictionaryProgress
            {
                UserId = userId,
                WordPairId = wordPairId,
                FirstSeenUtc = DateTime.UtcNow,
                LastSeenUtc = DateTime.UtcNow
            };
        }

        progress.Attempts++;

        if (isCorrect)
        {
            progress.CorrectAttempts++;
        }

        progress.LastSeenUtc = DateTime.UtcNow;

        await _progressCollection.ReplaceOneAsync(
            item => item.UserId == userId && item.WordPairId == wordPairId,
            progress,
            new ReplaceOptions { IsUpsert = true },
            cancellationToken);

        progress.WordPair = await _wordPairCollection
            .Find(item => item.Id == wordPairId)
            .FirstOrDefaultAsync(cancellationToken);

        return progress;
    }
}