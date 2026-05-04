using LinguaQuest.Web.Enums;

namespace LinguaQuest.Web.Components.Shared;

public static class LearningLevelLabels
{
    public static string GetLabel(LearningLevel level) => level switch
    {
        LearningLevel.Level1 => "1 - основи",
        LearningLevel.Level2 => "2 - прості слова",
        LearningLevel.Level3 => "3 - базові фрази",
        LearningLevel.Level4 => "4 - побут",
        LearningLevel.Level5 => "5 - школа",
        LearningLevel.Level6 => "6 - місто",
        LearningLevel.Level7 => "7 - подорожі",
        LearningLevel.Level8 => "8 - робота",
        LearningLevel.Level9 => "9 - спілкування",
        LearningLevel.Level10 => "10 - опис",
        LearningLevel.Level11 => "11 - історії",
        LearningLevel.Level12 => "12 - нюанси",
        LearningLevel.Level13 => "13 - абстракції",
        LearningLevel.Level14 => "14 - точність",
        LearningLevel.Level15 => "15 - майстер",
        _ => level.ToString()
    };
}
