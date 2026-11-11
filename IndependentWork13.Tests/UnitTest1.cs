
using System;
using System.Collections.Generic;
using IndependentWork13;
using Xunit;

namespace IndependentWork13.Tests;

public class CacheTests
{
    [Fact]
    public void Add_ShouldAddElement()
    {
        var cache = new Cache<int, string>(3);

        cache.Add(1, "яблуко");

        Assert.True(cache.ContainsKey(1));
    }

    [Fact]
    public void Get_ShouldReturnValue()
    {
        var cache = new Cache<int, string>(3);
        cache.Add(1, "яблуко");

        Assert.Equal("яблуко", cache.Get(1));
    }

    [Fact]
    public void Add_ShouldRemoveOldestElement_WhenFull()
    {
        var cache = new Cache<int, string>(3);

        cache.Add(1, "яблуко");
        cache.Add(2, "банан");
        cache.Add(3, "апельсин");
        cache.Add(4, "груша");

        Assert.False(cache.ContainsKey(1));
        Assert.True(cache.ContainsKey(2));
        Assert.True(cache.ContainsKey(3));
        Assert.True(cache.ContainsKey(4));
    }

    [Fact]
    public void Get_ShouldThrowException_WhenKeyNotFound()
    {
        var cache = new Cache<int, string>(3);

        Assert.Throws<KeyNotFoundException>(() => cache.Get(10));
    }

    [Fact]
    public void ContainsKey_ShouldReturnCorrectResult()
    {
        var cache = new Cache<int, string>(3);
        cache.Add(1, "яблуко");

        Assert.True(cache.ContainsKey(1));
        Assert.False(cache.ContainsKey(2));
    }

    [Fact]
    public void Count_ShouldReturnNumberOfElements()
    {
        var cache = new Cache<int, string>(3);

        cache.Add(1, "яблуко");
        cache.Add(2, "банан");

        Assert.Equal(2, cache.Count);
    }

    [Fact]
    public void Count_ShouldNotExceedCapacity()
    {
        var cache = new Cache<int, string>(2);

        cache.Add(1, "яблуко");
        cache.Add(2, "банан");
        cache.Add(3, "апельсин");

        Assert.Equal(2, cache.Count);
    }

    [Fact]
    public void Add_ShouldUpdateExistingValue()
    {
        var cache = new Cache<int, string>(2);

        cache.Add(1, "яблуко");
        cache.Add(1, "груша");

        Assert.Equal("груша", cache.Get(1));
        Assert.Equal(1, cache.Count);
    }
}
