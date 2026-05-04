using LinguaQuest.Web.Enums;
using LinguaQuest.Web.Models;

namespace LinguaQuest.Web.Services.GameModes;

public static class SentenceTemplateBuilder
{
    public static string BuildPrompt(WordPair word)
    {
        return BuildSentence(word, blankOnly: true);
    }

    public static string BuildAnswer(WordPair word)
    {
        return BuildSentence(word, blankOnly: false);
    }

    private static string BuildSentence(WordPair word, bool blankOnly)
    {
        var category = word.Category.Trim().ToLowerInvariant();
        var target = word.TargetText.Trim();

        return word.TargetLanguage switch
        {
            LearningLanguage.English => BuildEnglishSentence(category, target, blankOnly),
            LearningLanguage.German => BuildGermanSentence(category, target, blankOnly),
            _ => blankOnly ? $"Я бачу ___" : $"Я бачу {target}."
        };
    }

    private static string BuildEnglishSentence(string category, string target, bool blankOnly)
    {
        return category switch
        {
            "home" => blankOnly ? $"I live in a ___." : $"I live in a {ArticleForEnglish(target)} {target}.",
            "food" => blankOnly ? $"I eat ___." : $"I eat {target}.",
            "animals" => blankOnly ? $"I see a ___." : $"I see a {target}.",
            "school" => blankOnly ? $"I study at ___." : $"I study at {target}.",
            "travel" => blankOnly ? $"I am at the ___." : $"I am at the {target}.",
            "communication" => blankOnly ? $"I ask a ___." : $"I ask a {target}.",
            "work" => blankOnly ? $"I work with a ___." : $"I work with a {target}.",
            "science" => blankOnly ? $"I use ___." : $"I use {target}.",
            "nature" => blankOnly ? $"I see ___." : $"I see {target}.",
            "social" => blankOnly ? $"I meet a ___." : $"I meet a {target}.",
            "daily" => blankOnly ? $"I need ___." : $"I need {target}.",
            _ => blankOnly ? $"I see ___." : $"I see {target}."
        };
    }

    private static string BuildGermanSentence(string category, string target, bool blankOnly)
    {
        return category switch
        {
            "home" => blankOnly ? $"Ich wohne in einem ___." : $"Ich wohne in einem {target}.",
            "food" => blankOnly ? $"Ich esse ___." : $"Ich esse {target}.",
            "animals" => blankOnly ? $"Ich sehe einen ___." : $"Ich sehe einen {target}.",
            "school" => blankOnly ? $"Ich lerne in der ___." : $"Ich lerne in der {target}.",
            "travel" => blankOnly ? $"Ich bin am ___." : $"Ich bin am {target}.",
            "communication" => blankOnly ? $"Ich stelle eine ___." : $"Ich stelle eine {target}.",
            "work" => blankOnly ? $"Ich arbeite mit einem ___." : $"Ich arbeite mit einem {target}.",
            "science" => blankOnly ? $"Ich benutze ___." : $"Ich benutze {target}.",
            "nature" => blankOnly ? $"Ich sehe einen ___." : $"Ich sehe einen {target}.",
            "social" => blankOnly ? $"Ich treffe einen ___." : $"Ich treffe einen {target}.",
            "daily" => blankOnly ? $"Ich brauche ___." : $"Ich brauche {target}.",
            _ => blankOnly ? $"Ich sehe ___." : $"Ich sehe {target}."
        };
    }

    private static string ArticleForEnglish(string word)
    {
        return StartsWithVowelSound(word) ? "an" : "a";
    }

    private static bool StartsWithVowelSound(string word)
    {
        return !string.IsNullOrWhiteSpace(word) && "aeiouAEIOU".Contains(word[0]);
    }
}
