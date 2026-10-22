
using System;
using System.Collections.Generic;
using Xunit;
using IndependentWork7;

namespace IndependentWork7.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void AddTest()
        {
            var cache = new Cache<int, string>(2);
            cache.Add(1, "Книга");
            Assert.True(cache.ContainsKey(1));
        }

        [Fact]
        public void GetTest()
        {
            var cache = new Cache<int, string>(2);
            cache.Add(1, "Книга");
            Assert.Equal("Книга", cache.Get(1));
        }

        [Fact]
        public void FifoTest()
        {
            var cache = new Cache<int, string>(2);
            cache.Add(1, "Книга");
            cache.Add(2, "Журнал");
            cache.Add(3, "Газета");

            Assert.False(cache.ContainsKey(1));
            Assert.True(cache.ContainsKey(2));
            Assert.True(cache.ContainsKey(3));
        }

        [Fact]
        public void MissingKeyTest()
        {
            var cache = new Cache<int, string>(2);
            Assert.Throws<KeyNotFoundException>(() => cache.Get(5));
        }

        [Fact]
        public void ContainsKeyTest()
        {
            var cache = new Cache<int, string>(2);
            cache.Add(1, "Книга");

            Assert.True(cache.ContainsKey(1));
            Assert.False(cache.ContainsKey(2));
        }

        [Fact]
        public void CountTest()
        {
            var cache = new Cache<int, string>(2);
            cache.Add(1, "Книга");
            cache.Add(2, "Журнал");

            Assert.Equal(2, cache.Count);

            cache.Add(3, "Газета");
            Assert.Equal(2, cache.Count);
        }
    }
}
