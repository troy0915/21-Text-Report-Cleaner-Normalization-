using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class TextCleaner
{
    private static readonly HashSet<string> Stopwords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "the", "and", "a", "an", "in", "on", "of", "to", "is", "it", "for", "with", "at", "by", "from", "as", "that"
    };

    // 1. Trim whitespace from start and end
    public string TrimText(string text)
    {
        return text.Trim();
    }

    // 2. Condense multiple spaces into a single space
    public string CondenseSpaces(string text)
    {
        return Regex.Replace(text, @"\s+", " ");
    }

    // 3. Remove non-printable and non-ASCII characters
    public string RemoveNonPrintable(string text)
    {
        return Regex.Replace(text, @"[^\u0020-\u007E\s]", "");
    }

    // 4. Convert to Title Case based on rules
    public string ToTitleCase(string text)
    {
        TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
        return textInfo.ToTitleCase(text.ToLower());
    }

   
    public int CountWords(string text)
    {
        return Regex.Matches(text, @"\b\w+\b").Count;
    }

   
    public int CountSentences(string text)
    {
        return Regex.Matches(text, @"[.!?]+").Count;
    }

   
    public List<(string Word, int Count)> GetTopWords(string text, int topN = 5)
    {
        var words = Regex.Matches(text.ToLower(), @"\b\w+\b")
            .Cast<Match>()
            .Select(m => m.Value)
            .Where(w => !Stopwords.Contains(w));

        return words
            .GroupBy(w => w)
            .Select(g => (Word: g.Key, Count: g.Count()))
            .OrderByDescending(x => x.Count)
            .ThenBy(x => x.Word)
            .Take(topN)
            .ToList();
    }
}

namespace _21__Text_Report_Cleaner__Normalization_
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string messyText = @"
            ThiS     is    an   EXAMPLE text!! 
            It   has   Extra   SPACES,   miXed   CASE,  
            non-printable chars: ☺♥♦, and  some STOPWORDS to ignore.
        ";

            TextCleaner cleaner = new TextCleaner();

            string cleaned = messyText;
            cleaned = cleaner.TrimText(cleaned);
            cleaned = cleaner.CondenseSpaces(cleaned);
            cleaned = cleaner.RemoveNonPrintable(cleaned);
            cleaned = cleaner.ToTitleCase(cleaned);

            Console.WriteLine("Cleaned Text:");
            Console.WriteLine(cleaned);
            Console.WriteLine();

            int wordCount = cleaner.CountWords(cleaned);
            int sentenceCount = cleaner.CountSentences(cleaned);
            var topWords = cleaner.GetTopWords(cleaned);

            Console.WriteLine($"Word Count: {wordCount}");
            Console.WriteLine($"Sentence Count: {sentenceCount}");
            Console.WriteLine("Top 5 Words (ignoring stopwords):");

            foreach (var (Word, Count) in topWords)
            {
                Console.WriteLine($"{Word}: {Count}");
            }
        }
    }
}




