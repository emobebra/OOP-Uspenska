using System;
using System.Collections.Generic;
using System.Linq;

public class FileMetadata
{
    public string FileName { get; set; } = "";
    public long Size { get; set; }
    public DateTime LastModified { get; set; }
    public string Hash { get; set; } = "";
}

public class FileMetadataCache
{
    private Dictionary<string, FileMetadata> _cache = new();

    public void Add(string path, FileMetadata metadata)
    {
        _cache[path] = metadata;
    }

    public FileMetadata? Get(string path)
    {
        return _cache.TryGetValue(path, out var metadata) ? metadata : null;
    }

    public bool Remove(string path)
    {
        return _cache.Remove(path);
    }

    public bool Contains(string path)
    {
        return _cache.ContainsKey(path);
    }
}

public class TagManager
{
    private Dictionary<string, HashSet<string>> _articleTags = new();

    public void AddTag(string articleId, string tag)
    {
        if (!_articleTags.ContainsKey(articleId))
            _articleTags[articleId] = new HashSet<string>();

        _articleTags[articleId].Add(tag);
    }

    public void RemoveTag(string articleId, string tag)
    {
        if (_articleTags.TryGetValue(articleId, out var tags))
            tags.Remove(tag);
    }

    public HashSet<string> GetAllUniqueTags()
    {
        HashSet<string> result = new();

        foreach (var tags in _articleTags.Values)
            result.UnionWith(tags);

        return result;
    }

    public HashSet<string> FindCommonTags(string articleId1, string articleId2)
    {
        if (!_articleTags.ContainsKey(articleId1) ||
            !_articleTags.ContainsKey(articleId2))
            return new HashSet<string>();

        HashSet<string> result = new(_articleTags[articleId1]);
        result.IntersectWith(_articleTags[articleId2]);

        return result;
    }
}

public class SearchIndex
{
    private Dictionary<string, HashSet<int>> _invertedIndex =
        new(StringComparer.OrdinalIgnoreCase);

    public void AddDocument(int docId, string content)
    {
        string[] words = content.Split(
            new[] { ' ', ',', '.', '!', '?', '\n', '\t' },
            StringSplitOptions.RemoveEmptyEntries);

        foreach (string word in words)
        {
            if (!_invertedIndex.ContainsKey(word))
                _invertedIndex[word] = new HashSet<int>();

            _invertedIndex[word].Add(docId);
        }
    }

    public HashSet<int> Search(string word)
    {
        return _invertedIndex.TryGetValue(word, out var ids)
            ? new HashSet<int>(ids)
            : new HashSet<int>();
    }

    public HashSet<int> SearchMultiple(params string[] words)
    {
        if (words.Length == 0)
            return new HashSet<int>();

        HashSet<int> result = Search(words[0]);

        foreach (string word in words.Skip(1))
            result.IntersectWith(Search(word));

        return result;
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("1. кеш метаданих");

        FileMetadataCache cache = new();
        cache.Add("file.txt", new FileMetadata
        {
            FileName = "file.txt",
            Size = 1024,
            LastModified = DateTime.Now,
            Hash = "abc123"
        });

        Console.WriteLine("файл існує: " + cache.Contains("file.txt"));
        Console.WriteLine("розмір: " + cache.Get("file.txt")?.Size);
        cache.Remove("file.txt");
        Console.WriteLine("після видалення: " + cache.Contains("file.txt"));

        Console.WriteLine("\n2. теги статей");

        TagManager tags = new();
        tags.AddTag("1", "C#");
        tags.AddTag("1", "ООП");
        tags.AddTag("2", "C#");
        tags.AddTag("2", ".NET");

        Console.WriteLine("усі теги: " +
            string.Join(", ", tags.GetAllUniqueTags().OrderBy(x => x)));
        Console.WriteLine("спільні теги: " +
            string.Join(", ", tags.FindCommonTags("1", "2")));
        tags.RemoveTag("1", "ООП");

        Console.WriteLine("\n3. пошуковий індекс");

        SearchIndex index = new();
        index.AddDocument(1, "C# програмування ООП");
        index.AddDocument(2, "C# колекції dictionary");
        index.AddDocument(3, "python");

        Console.WriteLine("пошук C#: " +
            string.Join(", ", index.Search("C#").OrderBy(x => x)));
        Console.WriteLine("пошук C# і програмування: " +
            string.Join(", ", index.SearchMultiple("C#", "програмування").OrderBy(x => x)));
    }
}
