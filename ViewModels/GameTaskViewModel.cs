using LinguaQuest.Web.Enums;

namespace LinguaQuest.Web.ViewModels;

public class GameTaskViewModel
{
    public int WordPairId { get; set; }
    public GameModeType Mode { get; set; }
    public LearningLanguage SourceLanguage { get; set; }
    public LearningLanguage TargetLanguage { get; set; }
    public LearningLevel Level { get; set; }
    public string Prompt { get; set; } = string.Empty;
    public string CorrectAnswer { get; set; } = string.Empty;
    public string SourceText { get; set; } = string.Empty;
    public string TargetText { get; set; } = string.Empty;
    public string Hint { get; set; } = string.Empty;
    public string Explanation { get; set; } = string.Empty;
    public IReadOnlyList<string> Options { get; set; } = Array.Empty<string>();
}