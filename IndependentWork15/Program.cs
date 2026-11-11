using System;
using System.Collections.Generic;

List<int> numbers = new() { 5, 12, 8, 20, 3 };
List<string> words = new() { "Яблуко", "Банан", "Ківі", "Апельсин" };

List<Product> products = new()
{
    new Product { Name = "Ноутбук", Price = 25000 },
    new Product { Name = "Миша", Price = 1500 },
    new Product { Name = "гра в стімі", Price = 500 }
};

Console.WriteLine("метод map");

var doubled = CollectionUtils.Map(numbers, n => n * 2);
Console.WriteLine("Подвоєні числа: " + string.Join(", ", doubled));

var lengths = CollectionUtils.Map(words, w => w.Length);
Console.WriteLine("Довжини слів: " + string.Join(", ", lengths));

var names = CollectionUtils.Map(products, p => p.Name);
Console.WriteLine("Назви товарів: " + string.Join(", ", names));

Console.WriteLine("\n метод maxby");

Console.WriteLine("Найбільше число: " +
    CollectionUtils.MaxBy(numbers, n => n));

Console.WriteLine("Найдовше слово: " +
    CollectionUtils.MaxBy(words, w => w.Length));

var expensive = CollectionUtils.MaxBy(products, p => p.Price);
Console.WriteLine($"Найдорожчий товар: {expensive.Name} - {expensive.Price} грн");

Console.WriteLine("\n метод filter");

var even = CollectionUtils.Filter(numbers, n => n % 2 == 0);
Console.WriteLine("Парні числа: " + string.Join(", ", even));

var longWords = CollectionUtils.Filter(words, w => w.Length > 5);
Console.WriteLine("Слова довші за 5 символів: " + string.Join(", ", longWords));

var filteredProducts = CollectionUtils.Filter(products, p => p.Price > 1000);
Console.WriteLine("Товари дорожчі за 1000 грн:");
CollectionUtils.ForEach(filteredProducts,
    p => Console.WriteLine($"{p.Name}: {p.Price} грн"));

Console.WriteLine("\n метод foreach");

Console.WriteLine("Усі числа:");
CollectionUtils.ForEach(numbers, n => Console.WriteLine(n));

Console.WriteLine("Усі слова:");
CollectionUtils.ForEach(words, w => Console.WriteLine(w));

Console.WriteLine("Усі товари:");
CollectionUtils.ForEach(products,
    p => Console.WriteLine($"{p.Name}: {p.Price} грн"));
