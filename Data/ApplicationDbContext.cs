using LinguaQuest.Web.Models;
using MongoDB.Driver;

namespace LinguaQuest.Web.Data;

public sealed class ApplicationDbContext
{
	public ApplicationDbContext(IConfiguration configuration)
	{
		var connectionString = configuration["MongoDb:ConnectionString"] ?? "mongodb://localhost:27017";
		var databaseName = configuration["MongoDb:DatabaseName"] ?? "LinguaQuestDb";

		var client = new MongoClient(connectionString);
		Database = client.GetDatabase(databaseName);
	}

	public IMongoDatabase Database { get; }

	public IMongoCollection<ApplicationUser> Users => Database.GetCollection<ApplicationUser>("users");
	public IMongoCollection<WordPair> WordPairs => Database.GetCollection<WordPair>("wordPairs");
	public IMongoCollection<DictionaryProgress> DictionaryProgress => Database.GetCollection<DictionaryProgress>("dictionaryProgress");
	public IMongoCollection<UserLearningSettings> UserLearningSettings => Database.GetCollection<UserLearningSettings>("userLearningSettings");
	public IMongoCollection<GameSession> GameSessions => Database.GetCollection<GameSession>("gameSessions");
	public IMongoCollection<GameAnswer> GameAnswers => Database.GetCollection<GameAnswer>("gameAnswers");
}
