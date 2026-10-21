using System.Text;
using Lab8v3;

Console.OutputEncoding = Encoding.UTF8;

// List<int>: парні числа
var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
var evenNumbers = CollectionHelper<int>.Filter(numbers, n => n % 2 == 0);
Console.WriteLine("вихідні числа: " + string.Join(", ", numbers));
Console.WriteLine("парні числа:   " + string.Join(", ", evenNumbers));
Console.WriteLine();

// List<string>: рядки довші за 4 символи
var words = new List<string> { "кіт", "яблуко", "сонце", "ліс", "комп'ютер" };
var longWords = CollectionHelper<string>.Filter(words, w => w.Length > 4);
Console.WriteLine("вихідні рядки:    " + string.Join(", ", words));
Console.WriteLine("рядки довші за 4: " + string.Join(", ", longWords));
Console.WriteLine();

// ціна більше 100
var products = new List<Product>
{
    new Product(1, "ноутбук", 25000m, "електроніка"),
    new Product(2, "ручка", 15m, "канцелярія"),
    new Product(3, "навушники", 1200m, "улектроніка"),
    new Product(4, "зошит", 40m, "канцелярія"),
    new Product(5, "рюкзак", 850m, "аксесуари")
};
var expensive = CollectionHelper<Product>.Filter(products, p => p.Price > 100);
Console.WriteLine("продукти з ціною більше 100:");
foreach (var p in expensive)
{
    Console.WriteLine("  " + p);
}
Console.WriteLine();

// граничні випадки
var noMatches = CollectionHelper<int>.Filter(numbers, n => n > 100);
Console.WriteLine($"жоден елемент не підходить: знайдено {noMatches.Count()} елементів");

var emptyList = CollectionHelper<int>.Filter(new List<int>(), n => n > 0);
Console.WriteLine($"порожня колекція: знайдено {emptyList.Count()} елементів");
