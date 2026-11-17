using System.Diagnostics;

namespace InvertedIndexDemo;

public static class PerformanceTest
{
    public static void CompareSearchMethods(List<Document> documents, string searchWord)
    {
        var normalized = TextProcessor.Tokenize(searchWord).Single();
        var index = new InvertedIndex();
        foreach (var document in documents)
            index.AddDocument(document);

        // перший виклик прогріває JIT-компілятор.
        _ = documents.Where(d => TextProcessor.Tokenize(d.Content).Contains(normalized)).ToList();
        _ = index.SearchSingleWord(normalized);

        const int repetitions = 20;
        var sw = Stopwatch.StartNew();
        List<Document> linearResults = new();
        for (int i = 0; i < repetitions; i++)
        {
            linearResults = documents.Where(d =>
                TextProcessor.Tokenize(d.Content).Contains(normalized)).ToList();
        }
        sw.Stop();
        double linearTicks = (double)sw.ElapsedTicks / repetitions;

        sw.Restart();
        List<Document> indexResults = new();
        for (int i = 0; i < repetitions; i++)
            indexResults = index.SearchSingleWord(normalized);
        sw.Stop();
        double indexTicks = (double)sw.ElapsedTicks / repetitions;

        bool sameResults = linearResults.Select(d => d.Id).OrderBy(id => id)
            .SequenceEqual(indexResults.Select(d => d.Id).OrderBy(id => id));

        Console.WriteLine($"{documents.Count,6} | {linearTicks,14:F1} | {indexTicks,14:F1} | {sameResults}");
    }
}
