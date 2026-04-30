using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LinguaQuest.Web.Models;

public class DictionaryProgress
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
	public string UserId { get; set; } = string.Empty;
	public int WordPairId { get; set; }
	public int Attempts { get; set; }
	public int CorrectAttempts { get; set; }
	public DateTime FirstSeenUtc { get; set; } = DateTime.UtcNow;
	public DateTime LastSeenUtc { get; set; } = DateTime.UtcNow;

	[BsonIgnore]
	public int WrongAttempts => Math.Max(0, Attempts - CorrectAttempts);

	[BsonIgnore]
	public double Accuracy => Attempts == 0 ? 0 : CorrectAttempts * 100.0 / Attempts;

	[BsonIgnore]
	public bool IsLearned => Attempts >= 5 && Accuracy >= 70;

	[BsonIgnore]
	public WordPair? WordPair { get; set; }
}
