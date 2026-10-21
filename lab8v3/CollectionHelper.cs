namespace Lab8v3;

public static class CollectionHelper<T>
{
    public static IEnumerable<T> Filter(IEnumerable<T> collection, Predicate<T> predicate)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(predicate);

        var result = new List<T>();

        foreach (T item in collection)
        {
            if (predicate(item))
            {
                result.Add(item);
            }
        }

        return result;
    }
}