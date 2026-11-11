public static class CollectionUtils
{
    public static IEnumerable<TResult> Map<TSource, TResult>(
        IEnumerable<TSource> collection,
        Func<TSource, TResult> mapper)
    {
        var result = new List<TResult>();

        foreach (var item in collection)
        {
            result.Add(mapper(item));
        }

        return result;
    }

    public static T MaxBy<T, TKey>(
        IEnumerable<T> collection,
        Func<T, TKey> keySelector)
        where TKey : IComparable<TKey>
    {
        using var enumerator = collection.GetEnumerator();

        if (!enumerator.MoveNext())
            throw new InvalidOperationException("колекція порожня");

        T maxItem = enumerator.Current;
        TKey maxKey = keySelector(maxItem);

        while (enumerator.MoveNext())
        {
            T item = enumerator.Current;
            TKey key = keySelector(item);

            if (key.CompareTo(maxKey) > 0)
            {
                maxItem = item;
                maxKey = key;
            }
        }

        return maxItem;
    }

    public static IEnumerable<T> Filter<T>(
        IEnumerable<T> collection,
        Predicate<T> predicate)
    {
        var result = new List<T>();

        foreach (var item in collection)
        {
            if (predicate(item))
                result.Add(item);
        }

        return result;
    }

    public static void ForEach<T>(
        IEnumerable<T> collection,
        Action<T> action)
    {
        foreach (var item in collection)
        {
            action(item);
        }
    }
}
