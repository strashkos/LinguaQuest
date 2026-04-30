using LinguaQuest.Web.Models;

namespace LinguaQuest.Web.Services;

public static class ProgressCalculator
{
    public static double GetAccuracy(IEnumerable<DictionaryProgress> progress)
    {
        var attempts = progress.Sum(item => item.Attempts);
        var correct = progress.Sum(item => item.CorrectAttempts);
        return attempts == 0 ? 0 : correct * 100.0 / attempts;
    }

    public static bool IsLearned(DictionaryProgress progress)
    {
        return progress.Attempts >= 5 && progress.Accuracy >= 70;
    }
}