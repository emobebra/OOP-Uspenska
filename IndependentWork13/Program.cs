
using System;
using IndependentWork13;

Cache<string, string> cache = new Cache<string, string>(3);

cache.Add("1", "яблуко");
cache.Add("2", "банан");
cache.Add("3", "апельсин");

Console.WriteLine("початкова кількість елементів: " + cache.Count);
Console.WriteLine("елемент 1: " + cache.Get("1"));

cache.Add("4", "груша");

Console.WriteLine("\n після додавання четвертого елемента:");
Console.WriteLine("кількість елементів: " + cache.Count);
Console.WriteLine("чи існує ключ 1: " + cache.ContainsKey("1"));
Console.WriteLine("чи існує ключ 4: " + cache.ContainsKey("4"));

try
{
    Console.WriteLine(cache.Get("1"));
}
catch (KeyNotFoundException)
{
    Console.WriteLine("помилка: ключ 1 видалено за політикою FIFO");
}
