using LinguaQuest.Web.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LinguaQuest.Web.Models;

public class UserLearningSettings
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
	public string UserId { get; set; } = string.Empty;
	public LearningLanguage SourceLanguage { get; set; } = LearningLanguage.Ukrainian;
	public LearningLanguage TargetLanguage { get; set; } = LearningLanguage.English;
	public LearningLevel Level { get; set; } = LearningLevel.Level1;
	public int WordsPerSession { get; set; } = 5;
	public string ThemePreference { get; set; } = "dark";
}
