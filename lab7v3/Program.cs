using System;
using System.Collections.Generic;
using System.Text;

namespace lab7v3
{
    class Program
    {
        static void PrintAll(IRepository<Order> repo)
        {
            foreach (Order order in repo.GetAll())
            {
                Console.WriteLine(order);
            }
        }

        static void Main(string[] args)
        {
            IRepository<Order> repo = new InMemoryRepository<Order>();

            repo.Add(new Order { Id = 1, OrderDate = new DateTime(2025, 3, 10), TotalAmount = 1250.50m, CustomerId = 101 });
            repo.Add(new Order { Id = 2, OrderDate = new DateTime(2025, 3, 12), TotalAmount = 320m, CustomerId = 102 });
            repo.Add(new Order { Id = 3, OrderDate = new DateTime(2025, 3, 15), TotalAmount = 87.99m, CustomerId = 101 });

            Console.WriteLine("усі замовлення після додавання:");
            PrintAll(repo);

            Console.WriteLine();
            Console.WriteLine("замовлення з Id = 2:");
            Console.WriteLine(repo.GetById(2));

            repo.Update(new Order { Id = 2, OrderDate = new DateTime(2025, 3, 12), TotalAmount = 999.99m, CustomerId = 102 });
            Console.WriteLine();
            Console.WriteLine("після оновлення замовлення №2:");
            PrintAll(repo);

            repo.Delete(1);
            Console.WriteLine();
            Console.WriteLine("після видалення замовлення №1:");
            PrintAll(repo);

            Console.WriteLine();
            try
            {
                repo.GetById(1);
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine("спроба отримати видалений елемент: " + ex.Message);
            }
        }
    }
}