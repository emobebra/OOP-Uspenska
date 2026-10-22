using InvertedIndexDemo;

var documents = new List<Document>
{
    new() { Id = 1, Title = "Introduction to C#", Content = "C# is a modern object-oriented programming language developed by Microsoft." },
    new() { Id = 2, Title = "Java Basics", Content = "Java is a popular programming language known for its portability and performance." },
    new() { Id = 3, Title = "Python Overview", Content = "Python is a high-level programming language with simple syntax." },
    new() { Id = 4, Title = "C# Advanced Features", Content = "C# supports LINQ, async/await, and many modern programming paradigms." },
    new() { Id = 5, Title = "Comparing Languages", Content = "C#, Java, and Python are among the most popular programming languages today." }
};

var index = new InvertedIndex();
foreach (var document in documents)
    index.AddDocument(document);

Console.WriteLine($"Loaded {index.DocumentCount} documents");
Console.WriteLine($"Index contains {index.UniqueWords} unique words");

Console.WriteLine("\nPerformance test (average ticks per search):");
Console.WriteLine("  Docs |   Linear ticks |    Index ticks | Same results");
foreach (int count in new[] { 10, 100, 1000, 10000 })
{
    var sample = Enumerable.Range(1, count)
        .Select(i => new Document
        {
            Id = i,
            Title = $"Document {i}",
            Content = documents[(i - 1) % documents.Count].Content
        }).ToList();
    PerformanceTest.CompareSearchMethods(sample, "programming");
}

while (true)
{
    Console.Write("\nEnter search query (or 'exit'): ");
    string? query = Console.ReadLine();
    if (query == null || query.Trim().Equals("exit", StringComparison.OrdinalIgnoreCase))
        break;

    var words = TextProcessor.Tokenize(query).ToArray();
    var andResults = index.SearchMultipleWords(words);
    var orResults = index.SearchAnyWord(words);

    Console.WriteLine("\nSearch Results (AND)");
    foreach (var doc in andResults)
    {
        Console.WriteLine(doc);
        Console.WriteLine("  " + doc.Content);
    }
    Console.WriteLine($"Found {andResults.Count} documents");

    Console.WriteLine("\nSearch Results (OR)");
    foreach (var doc in orResults)
        Console.WriteLine(doc);
    Console.WriteLine($"Found {orResults.Count} documents");
}
