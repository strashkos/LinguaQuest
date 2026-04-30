using LinguaQuest.Web.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LinguaQuest.Web.Models;

public class GameAnswer
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
    public string GameSessionId { get; set; } = string.Empty;
    public int WordPairId { get; set; }
    public string SelectedAnswer { get; set; } = string.Empty;
    public AnswerResult Result { get; set; }
    public DateTime AnsweredUtc { get; set; } = DateTime.UtcNow;
}