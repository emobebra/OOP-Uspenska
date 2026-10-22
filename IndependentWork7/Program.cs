using System;
using IndependentWork7;

Cache<string, string> cache = new Cache<string, string>(3);

cache.Add("1", "книга");
cache.Add("2", "журнал");
cache.Add("3", "газета");

Console.WriteLine("елемент 2: " + cache.Get("2"));
Console.WriteLine("кількість: " + cache.Count);

//додаю четвертий елемент, тому перший видаляється
cache.Add("4", "зошит");

Console.WriteLine("чи є елемент 1: " + cache.ContainsKey("1"));
Console.WriteLine("елемент 4: " + cache.Get("4"));
Console.WriteLine("кількість: " + cache.Count);
