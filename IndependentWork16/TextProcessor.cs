using System.Text.RegularExpressions;

namespace InvertedIndexDemo;

public static class TextProcessor
{
    public static IEnumerable<string> Tokenize(string text)
    {
        return Regex.Matches(text.ToLowerInvariant(), @"[\p{L}\p{N}]+(?:#[\p{L}\p{N}]*)?")
            .Select(match => match.Value);
    }

    public static IEnumerable<string> RemoveStopWords(IEnumerable<string> words)
    {
        var stopWords = new HashSet<string>
        {
            "a", "an", "the", "is", "are", "was", "were",
            "in", "on", "at", "to", "for", "of", "and", "or"
        };

        return words.Where(word => !stopWords.Contains(word));
    }
}
