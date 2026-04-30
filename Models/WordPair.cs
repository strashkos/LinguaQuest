using LinguaQuest.Web.Enums;

namespace LinguaQuest.Web.Models;

public class WordPair
{
	public int Id { get; set; }
	public LearningLanguage SourceLanguage { get; set; }
	public LearningLanguage TargetLanguage { get; set; }
	public LearningLevel Level { get; set; }
	public string SourceText { get; set; } = string.Empty;
	public string TargetText { get; set; } = string.Empty;
	public string Category { get; set; } = string.Empty;
	public string Hint { get; set; } = string.Empty;
}
