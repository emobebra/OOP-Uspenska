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
    private readonly Dictionary<string, FileMetadata> _cache = new();

    public void Add(string key, FileMetadata metadata)
    {
        _cache[key] = metadata;
    }

    public FileMetadata? Get(string key)
    {
        return _cache.TryGetValue(key, out var metadata) ? metadata : null;
    }

    public bool Remove(string key)
    {
        return _cache.Remove(key);
    }

    public bool Contains(string key)
    {
        return _cache.ContainsKey(key);
    }
}

public class TagManager
{
    private readonly Dictionary<string, HashSet<string>> _articleTags = new();

    public void AddTag(string articleId, string tag)
    {
        if (!_articleTags.ContainsKey(articleId))
        {
            _articleTags[articleId] = new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);
        }

        _articleTags[articleId].Add(tag);
    }

    public void RemoveTag(string articleId, string tag)
    {
        if (_articleTags.TryGetValue(articleId, out var tags))
        {
            tags.Remove(tag);
        }
    }

    public HashSet<string> GetAllUniqueTags()
    {
        var result = new HashSet<string>(
            StringComparer.OrdinalIgnoreCase);

        foreach (var tags in _articleTags.Values)
        {
            result.UnionWith(tags);
        }

        return result;
    }

    public HashSet<string> FindCommonTags(string articleId1, string articleId2)
    {
        var result = new HashSet<string>(
            StringComparer.OrdinalIgnoreCase);

        if (_articleTags.TryGetValue(articleId1, out var tags1) &&
            _articleTags.TryGetValue(articleId2, out var tags2))
        {
            result.UnionWith(tags1);
            result.IntersectWith(tags2);
        }

        return result;
    }

    public HashSet<string> FindDifferentTags(string articleId1, string articleId2)
    {
        var result = new HashSet<string>(
            StringComparer.OrdinalIgnoreCase);

        if (_articleTags.TryGetValue(articleId1, out var tags1))
        {
            result.UnionWith(tags1);

            if (_articleTags.TryGetValue(articleId2, out var tags2))
            {
                result.ExceptWith(tags2);
            }
        }

        return result;
    }
}

public class Program
{
    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("Сценарій 1: Кеш метаданих (Dictionary)");

        var cache = new FileMetadataCache();

        cache.Add("file1", new FileMetadata
        {
            FileName = "document.txt",
            Size = 1024,
            LastModified = new DateTime(2026, 11, 1),
            Hash = "ABC123"
        });

        cache.Add("file2", new FileMetadata
        {
            FileName = "image.png",
            Size = 2048,
            LastModified = new DateTime(2026, 11, 2),
            Hash = "XYZ456"
        });

        var file = cache.Get("file1");

        if (file != null)
        {
            Console.WriteLine($"назва: {file.FileName}");
            Console.WriteLine($"розмір: {file.Size} байт");
            Console.WriteLine($"дата зміни: {file.LastModified:dd.MM.yyyy}");
            Console.WriteLine($"хеш: {file.Hash}");
        }

        Console.WriteLine($"файл існує: {cache.Contains("file1")}");
        Console.WriteLine($"видалення: {cache.Remove("file1")}");
        Console.WriteLine($"після видалення: {cache.Contains("file1")}");

        Console.WriteLine("\n Сценарій 2: Унікальні теги (HashSet)");

        var manager = new TagManager();

        manager.AddTag("article1", "csharp");
        manager.AddTag("article1", "oop");
        manager.AddTag("article1", "dotnet");

        manager.AddTag("article2", "csharp");
        manager.AddTag("article2", "programming");
        manager.AddTag("article2", "oop");

        manager.AddTag("article1", "csharp");

        Console.WriteLine("усі унікальні теги: " +
            string.Join(", ", manager.GetAllUniqueTags().OrderBy(x => x)));

        Console.WriteLine("спільні теги: " +
            string.Join(", ", manager.FindCommonTags("article1", "article2").OrderBy(x => x)));

        Console.WriteLine("теги лише першої статті: " +
            string.Join(", ", manager.FindDifferentTags("article1", "article2").OrderBy(x => x)));

        manager.RemoveTag("article1", "dotnet");

        Console.WriteLine("після видалення dotnet: " +
            string.Join(", ", manager.GetAllUniqueTags().OrderBy(x => x)));
    }
}