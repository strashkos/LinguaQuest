namespace LinguaQuest.Web.ViewModels;

public class GameResultViewModel
{
    public bool IsCorrect { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Explanation { get; set; } = string.Empty;
    public string SelectedAnswer { get; set; } = string.Empty;
    public string CorrectAnswer { get; set; } = string.Empty;
}