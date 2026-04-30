using LinguaQuest.Web.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LinguaQuest.Web.Models;

public class GameSession
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
    public string UserId { get; set; } = string.Empty;
    public GameModeType Mode { get; set; }
    public DateTime StartedUtc { get; set; } = DateTime.UtcNow;
    public bool IsCompleted { get; set; }
    public int CorrectAnswers { get; set; }
    public int TotalAnswers { get; set; }
    public List<GameAnswer> Answers { get; set; } = new();
}