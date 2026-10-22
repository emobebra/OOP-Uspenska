using System.Diagnostics;

namespace IndependentWork11;

public class Program
{
    static void Main()
    {
        var random = new Random(11);
        string[] categories = { "Electronics", "Clothing", "Books", "Home", "Sports" };
        string[] cities = { "Rivne", "Kyiv", "Lviv", "Odesa", "Dnipro" };

        var products = Enumerable.Range(1, 100)
            .Select(id => new Product
            {
                Id = id,
                Name = $"Product {id}",
                Category = categories[(id - 1) % categories.Length],
                UnitPrice = random.Next(50, 2001)
            }).ToList();

        var customers = Enumerable.Range(1, 100)
            .Select(id => new Customer
            {
                Id = id,
                Name = $"Customer {id}",
                City = cities[random.Next(cities.Length)]
            }).ToList();

        var sales = Enumerable.Range(1, 1000)
            .Select(id => new Sale
            {
                Id = id,
                ProductId = random.Next(1, 101),
                CustomerId = random.Next(1, 101),
                Quantity = random.Next(1, 11),
                SaleDate = new DateTime(2025, 1, 1).AddDays(random.Next(0, 730))
            }).ToList();

        Console.WriteLine($"Products: {products.Count}, Customers: {customers.Count}, Sales: {sales.Count}");
        Measure("1. Top 3 products in each category", () => TopProductsByCategory(sales, products));
        Measure("2. Revenue by city", () => RevenueByCity(sales, products, customers));
        Measure("3. Sales by year and month", () => SalesByMonth(sales, products));
        Measure("4. Top customers by revenue in 2026", () => TopCustomersIn2026(sales, products, customers));
    }

    static void Measure(string title, Action pipeline)
    {
        Console.WriteLine($"\n{title}");
        var stopwatch = Stopwatch.StartNew();
        pipeline();
        stopwatch.Stop();
        Console.WriteLine($"Execution time: {stopwatch.Elapsed.TotalMilliseconds:F3} ms ({stopwatch.ElapsedTicks} ticks)");
    }

    static void TopProductsByCategory(List<Sale> sales, List<Product> products)
    {
        var result = sales
            .GroupBy(s => s.ProductId)
            .Select(g => new { ProductId = g.Key, Units = g.Sum(s => s.Quantity) })
            .Join(products, s => s.ProductId, p => p.Id,
                (s, p) => new { p.Name, p.Category, s.Units })
            .GroupBy(x => x.Category)
            .OrderBy(g => g.Key)
            .Select(g => new { Category = g.Key, Top = g.OrderByDescending(x => x.Units).ThenBy(x => x.Name).Take(3).ToList() })
            .ToList();

        foreach (var category in result)
        {
            Console.WriteLine($"{category.Category}:");
            foreach (var item in category.Top)
                Console.WriteLine($"  {item.Name}: {item.Units} units");
        }
    }

    static void RevenueByCity(List<Sale> sales, List<Product> products, List<Customer> customers)
    {
        var result = sales
            .Join(products, s => s.ProductId, p => p.Id,
                (s, p) => new { s.CustomerId, Revenue = s.Quantity * p.UnitPrice })
            .Join(customers, x => x.CustomerId, c => c.Id,
                (x, c) => new { c.City, x.Revenue })
            .GroupBy(x => x.City)
            .Select(g => new { City = g.Key, Revenue = g.Sum(x => x.Revenue) })
            .OrderByDescending(x => x.Revenue)
            .ToList();

        foreach (var item in result)
            Console.WriteLine($"{item.City}: {item.Revenue:F2}");
    }

    static void SalesByMonth(List<Sale> sales, List<Product> products)
    {
        var result = sales
            .Join(products, s => s.ProductId, p => p.Id,
                (s, p) => new { s.SaleDate, s.Quantity, Revenue = s.Quantity * p.UnitPrice })
            .GroupBy(x => new { x.SaleDate.Year, x.SaleDate.Month })
            .Select(g => new { g.Key.Year, g.Key.Month, Units = g.Sum(x => x.Quantity), Revenue = g.Sum(x => x.Revenue) })
            .OrderBy(x => x.Year).ThenBy(x => x.Month)
            .ToList();

        foreach (var item in result)
            Console.WriteLine($"{item.Year}-{item.Month:D2}: {item.Units} units, revenue {item.Revenue:F2}");
    }

    static void TopCustomersIn2026(List<Sale> sales, List<Product> products, List<Customer> customers)
    {
        var result = sales
            .Where(s => s.SaleDate.Year == 2026)
            .Join(products, s => s.ProductId, p => p.Id,
                (s, p) => new { s.CustomerId, Revenue = s.Quantity * p.UnitPrice })
            .GroupBy(x => x.CustomerId)
            .Select(g => new { CustomerId = g.Key, Revenue = g.Sum(x => x.Revenue) })
            .Join(customers, x => x.CustomerId, c => c.Id,
                (x, c) => new { c.Name, c.City, x.Revenue })
            .OrderByDescending(x => x.Revenue)
            .Take(5)
            .ToList();

        foreach (var item in result)
            Console.WriteLine($"{item.Name} ({item.City}): {item.Revenue:F2}");
    }
}