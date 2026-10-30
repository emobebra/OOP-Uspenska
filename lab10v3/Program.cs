using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

class Author
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
}

class Post
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";
    public int AuthorId { get; set; }
}

class Program
{
    static void Measure<T>(string title, Func<T> query, Action<T> print)
    {
        var watch = Stopwatch.StartNew();
        T result = query();
        watch.Stop();

        Console.WriteLine("\n" + title);
        print(result);
        Console.WriteLine($"час: {watch.Elapsed.TotalMilliseconds:F6} мс");
    }

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        List<Author> authors = new()
        {
            new() { Id = 1, Name = "Саша", Email = "sasha28@mail.com" },
            new() { Id = 2, Name = "Сергій", Email = "serhii@mail.com" },
            new() { Id = 3, Name = "Марія", Email = "maria@mail.com" },
            new() { Id = 4, Name = "Андрій", Email = "andriy@mail.com" },
            new() { Id = 5, Name = "Софія", Email = "sofia@mail.com" },
            new() { Id = 6, Name = "Максим", Email = "max@mail.com" },
            new() { Id = 7, Name = "Анна", Email = "anna@mail.com" },
            new() { Id = 8, Name = "Роман", Email = "roma9@mail.com" },
            new() { Id = 9, Name = "Маргарита", Email = "margo@mail.com" },
            new() { Id = 10, Name = "Олексій", Email = "oleksiy@mail.com" }
        };

        List<Post> posts = new()
        {
            new() { Id = 1, Title = "основи C#", Content = "вступ до програмування.", AuthorId = 1 },
            new() { Id = 2, Title = "що таке LINQ", Content = "LINQ дозволяє працювати з колекціями.", AuthorId = 2 },
            new() { Id = 3, Title = "колекції C#", Content = "робота зі списками та словниками.", AuthorId = 3 },
            new() { Id = 4, Title = "LINQ Where", Content = "Фільтрація даних за допомогою LINQ.", AuthorId = 1 },
            new() { Id = 5, Title = "Основи SQL", Content = "Запити до баз даних.", AuthorId = 4 },
            new() { Id = 6, Title = "LINQ Join", Content = "Об'єднання двох колекцій.", AuthorId = 5 },
            new() { Id = 7, Title = "Масиви", Content = "Створення та обробка масивів.", AuthorId = 6 },
            new() { Id = 8, Title = "ООП", Content = "Класи, об'єкти та наслідування.", AuthorId = 7 },
            new() { Id = 9, Title = "LINQ GroupBy", Content = "Групування елементів колекції.", AuthorId = 2 },
            new() { Id = 10, Title = "Методи C#", Content = "Створення власних методів.", AuthorId = 8 },
            new() { Id = 11, Title = "Делегати", Content = "Використання делегатів.", AuthorId = 9 },
            new() { Id = 12, Title = "Lambda", Content = "Лямбда-вирази у LINQ.", AuthorId = 3 },
            new() { Id = 13, Title = "Інтерфейси", Content = "Робота з інтерфейсами.", AuthorId = 10 },
            new() { Id = 14, Title = "LINQ Select", Content = "Вибір потрібних даних.", AuthorId = 4 },
            new() { Id = 15, Title = "Файли", Content = "Читання та запис файлів.", AuthorId = 5 },
            new() { Id = 16, Title = "Винятки", Content = "Обробка помилок програми.", AuthorId = 6 },
            new() { Id = 17, Title = "LINQ Sum", Content = "Обчислення суми елементів.", AuthorId = 7 },
            new() { Id = 18, Title = "Сортування", Content = "Сортування колекцій за різними умовами.", AuthorId = 8 },
            new() { Id = 19, Title = "LINQ Average", Content = "Обчислення середнього значення.", AuthorId = 9 },
            new() { Id = 20, Title = "Продуктивність", Content = "Порівняння часу виконання різних операцій LINQ за допомогою Stopwatch.", AuthorId = 10 }
        };


        Measure("1. фільтрація (синтаксис запитів)", () =>
            (from p in posts
             where p.Title.Contains("LINQ", StringComparison.OrdinalIgnoreCase)
                || p.Content.Contains("LINQ", StringComparison.OrdinalIgnoreCase)
             select p).ToList(),
            result =>
            {
                foreach (var p in result)
                    Console.WriteLine(p.Title);
            });

        Measure("1. фільтрація (синтаксис методів)", () =>
            posts.Where(p =>
                p.Title.Contains("LINQ", StringComparison.OrdinalIgnoreCase) ||
                p.Content.Contains("LINQ", StringComparison.OrdinalIgnoreCase)).ToList(),
            result =>
            {
                foreach (var p in result)
                    Console.WriteLine(p.Title);
            });

        Measure("2. join (синтаксис запитів)", () =>
            (from p in posts
             join a in authors on p.AuthorId equals a.Id
             select new { p.Title, a.Name }).ToList(),
            result =>
            {
                foreach (var item in result)
                    Console.WriteLine($"{item.Title} — {item.Name}");
            });

        Measure("2. join (синтаксис методів)", () =>
            posts.Join(authors,
                p => p.AuthorId,
                a => a.Id,
                (p, a) => new { p.Title, a.Name }).ToList(),
            result =>
            {
                foreach (var item in result)
                    Console.WriteLine($"{item.Title} — {item.Name}");
            });

        Measure("3. агрегація (синтаксис запитів)", () =>
            (from a in authors
             join p in posts on a.Id equals p.AuthorId into authorPosts
             select new
             {
                 a.Name,
                 Count = authorPosts.Count()
             }).ToList(),
            result =>
            {
                foreach (var item in result)
                    Console.WriteLine($"{item.Name}: {item.Count} постів");
            });

        Measure("3. агрегація (синтаксис методів)", () =>
            authors.GroupJoin(posts,
                a => a.Id,
                p => p.AuthorId,
                (a, authorPosts) => new
                {
                    a.Name,
                    Count = authorPosts.Count()
                }).ToList(),
            result =>
            {
                foreach (var item in result)
                    Console.WriteLine($"{item.Name}: {item.Count} постів");
            });

        Measure("4. найдовший пост (синтаксис запитів)", () =>
            (from p in posts
             orderby p.Content.Length descending
             select p).First(),
            result =>
            {
                Console.WriteLine($"{result.Title} — {result.Content.Length} символів");
            });

        Measure("4. найдовший пост (синтаксис методів)", () =>
            posts.OrderByDescending(p => p.Content.Length).First(),
            result =>
            {
                Console.WriteLine($"{result.Title} — {result.Content.Length} символів");
            });
    }
}
