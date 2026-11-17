namespace InvertedIndexDemo;

public class InvertedIndex
{
    private readonly Dictionary<string, HashSet<int>> _index = new();
    private readonly Dictionary<int, Document> _documents = new();

    public void AddDocument(Document doc)
    {
        if (_documents.ContainsKey(doc.Id))
            throw new ArgumentException("Document ID already exists");

        _documents[doc.Id] = doc;
        foreach (string word in TextProcessor.Tokenize(doc.Content).Distinct())
        {
            if (!_index.TryGetValue(word, out var ids))
            {
                ids = new HashSet<int>();
                _index[word] = ids;
            }
            ids.Add(doc.Id);
        }
    }

    public List<Document> SearchSingleWord(string word)
    {
        var words = TextProcessor.Tokenize(word).ToArray();
        if (words.Length != 1 || !_index.TryGetValue(words[0], out var ids))
            return new List<Document>();

        return ids.OrderBy(id => id).Select(id => _documents[id]).ToList();
    }

    public List<Document> SearchMultipleWords(params string[] words)
    {
        var tokens = words.SelectMany(TextProcessor.Tokenize).Distinct().ToArray();
        if (tokens.Length == 0)
            return new List<Document>();

        var result = new HashSet<int>();
        bool first = true;
        foreach (string word in tokens)
        {
            if (!_index.TryGetValue(word, out var ids))
                return new List<Document>();

            if (first)
            {
                result.UnionWith(ids);
                first = false;
            }
            else
            {
                result.IntersectWith(ids);
            }
        }

        return result.OrderBy(id => id).Select(id => _documents[id]).ToList();
    }

    public List<Document> SearchAnyWord(params string[] words)
    {
        var result = new HashSet<int>();
        foreach (string word in words.SelectMany(TextProcessor.Tokenize))
        {
            if (_index.TryGetValue(word, out var ids))
                result.UnionWith(ids);
        }

        return result.OrderBy(id => id).Select(id => _documents[id]).ToList();
    }

    public int DocumentCount => _documents.Count;
    public int UniqueWords => _index.Count;
}
