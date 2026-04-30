using LinguaQuest.Web.Enums;

namespace LinguaQuest.Web.ViewModels;

public class DictionaryItemViewModel
{
    public int Id { get; set; }
    public string SourceText { get; set; } = string.Empty;
    public string TargetText { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Hint { get; set; } = string.Empty;
    public LearningLevel Level { get; set; }
}