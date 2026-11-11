
using System;
using System.Collections.Generic;

namespace IndependentWork13;

public class Cache<TKey, TValue> where TKey : notnull
{
    private readonly int _capacity;
    private readonly Dictionary<TKey, TValue> _cache;
    private readonly Queue<TKey> _keysOrder;

    public Cache(int capacity)
    {
        if (capacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacity));

        _capacity = capacity;
        _cache = new Dictionary<TKey, TValue>();
        _keysOrder = new Queue<TKey>();
    }

    public void Add(TKey key, TValue value)
    {
        if (_cache.ContainsKey(key))
        {
            _cache[key] = value;
            return;
        }

        if (_cache.Count >= _capacity)
        {
            TKey oldestKey = _keysOrder.Dequeue();
            _cache.Remove(oldestKey);
        }

        _cache.Add(key, value);
        _keysOrder.Enqueue(key);
    }

    public TValue Get(TKey key)
    {
        if (_cache.TryGetValue(key, out var value))
            return value;

        throw new KeyNotFoundException("Ключ не знайдено в кеші.");
    }

    public bool ContainsKey(TKey key)
    {
        return _cache.ContainsKey(key);
    }

    public int Count
    {
        get { return _cache.Count; }
    }
}
