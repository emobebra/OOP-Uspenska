
using System;
using System.Collections.Generic;

public class Product
{
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
}

public static class CollectionUtils
{
    public static IEnumerable<TResult> Map<TSource, TResult>(
        IEnumerable<TSource> collection, Func<TSource, TResult> mapper)
    {
        var result = new List<TResult>();

        foreach (var item in collection)
        {
            result.Add(mapper(item));
        }

        return result;
    }

    public static T MaxBy<T, TKey>(
        IEnumerable<T> collection, Func<T, TKey> keySelector)
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
                maxKey = key;
                maxItem = item;
            }
        }

        return maxItem;
    }

    public static IEnumerable<T> Filter<T>(
        IEnumerable<T> collection, Predicate<T> predicate)
    {
        var result = new List<T>();

        foreach (var item in collection)
        {
            if (predicate(item))
            {
                result.Add(item);
            }
        }

        return result;
    }

    public static void ForEach<T>(
        IEnumerable<T> collection, Action<T> action)
    {
        foreach (var item in collection)
        {
            action(item);
        }
    }
}

class Program
{
    static void Main()
    {
        var numbers = new List<int> { 2, 5, 8, 11, 14 };

        Console.WriteLine("числа:");
        var doubled = CollectionUtils.Map(numbers, n => n * 2);
        Console.WriteLine("подвоєні: " + string.Join(", ", doubled));

        var even = CollectionUtils.Filter(numbers, n => n % 2 == 0);
        Console.WriteLine("парні: " + string.Join(", ", even));

        Console.WriteLine("найбільше число: " +
            CollectionUtils.MaxBy(numbers, n => n));

        CollectionUtils.ForEach(numbers, n => Console.Write(n + " "));
        Console.WriteLine();

        var words = new List<string> { "книга", "автомобіль", "програмування" };

        Console.WriteLine("\nслова:");
        var lengths = CollectionUtils.Map(words, w => w.Length);
        Console.WriteLine("довжини: " + string.Join(", ", lengths));

        var longWords = CollectionUtils.Filter(words, w => w.Length > 4);
        Console.WriteLine("довгі слова: " + string.Join(", ", longWords));

        Console.WriteLine("найдовше слово: " +
            CollectionUtils.MaxBy(words, w => w.Length));

        CollectionUtils.ForEach(words, w => Console.WriteLine(w));

        var products = new List<Product>
        {
            new Product { Name = "ноут", Price = 35000 },
            new Product { Name = "мишка", Price = 100 },
            new Product { Name = "клавіатура", Price = 2500 }
        };

        Console.WriteLine("\nтовари:");
        var names = CollectionUtils.Map(products, p => p.Name);
        Console.WriteLine("назви: " + string.Join(", ", names));

        var expensive = CollectionUtils.Filter(products, p => p.Price > 1000);
        Console.WriteLine("товари дорожчі > 1000:");
        CollectionUtils.ForEach(expensive,
            p => Console.WriteLine(p.Name));

        var mostExpensive = CollectionUtils.MaxBy(products, p => p.Price);
        Console.WriteLine("найдорожчий: " + mostExpensive.Name);

        CollectionUtils.ForEach(products,
            p => Console.WriteLine($"{p.Name}: {p.Price} грн"));
    }
}

