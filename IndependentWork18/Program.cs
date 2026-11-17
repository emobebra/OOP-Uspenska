using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

class Program
{
    static double Calculate(int number)
    {
        double result = number;

        for (int i = 0; i < 20; i++)
        {
            result = Math.Sqrt(result + i + 1)
                   + Math.Sin(result * 0.01);
        }

        return result;
    }

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        int[] sizes = { 1_000_000, 10_000_000 };
        Random random = new Random(42);
        Stopwatch stopwatch = new Stopwatch();

        foreach (int size in sizes)
        {
            Console.WriteLine($"\nколекція: {size:N0} елементів");

            List<int> numbers = new List<int>(size);

            for (int i = 0; i < size; i++)
            {
                numbers.Add(random.Next(1, 100000));
            }

            stopwatch.Restart();

            double[] linqResult = numbers
                .Where(x => x % 2 == 0)
                .Select(x => Calculate(x))
                .ToArray();

            stopwatch.Stop();
            long linqTime = stopwatch.ElapsedMilliseconds;

            stopwatch.Restart();

            double[] plinqResult = numbers
                .AsParallel()
                .Where(x => x % 2 == 0)
                .Select(x => Calculate(x))
                .ToArray();

            stopwatch.Stop();
            long plinqTime = stopwatch.ElapsedMilliseconds;

            Console.WriteLine($"LINQ:  {linqTime} мс");
            Console.WriteLine($"PLINQ: {plinqTime} мс");

            Console.WriteLine($"LINQ результатів:  {linqResult.Length}");
            Console.WriteLine($"PLINQ результатів: {plinqResult.Length}");

            if (plinqTime < linqTime)
            {
                Console.WriteLine("PLINQ працює швидше.");
            }
            else
            {
                Console.WriteLine("LINQ працює швидше.");
            }

            if (linqResult.Length != plinqResult.Length)
            {
                Console.WriteLine("помилка, кількість результатів відрізняється");
            }
        }

        Console.WriteLine("\nдослідження потокобезпечності");

        const int count = 1_000_000;

        for (int attempt = 1; attempt <= 5; attempt++)
        {
            int counter = 0;

            Enumerable.Range(0, count)
                .AsParallel()
                .WithDegreeOfParallelism(4)
                .ForAll(x =>
                {
                    counter++;
                });

            Console.WriteLine(
                $"без синхронізації, спроба {attempt}: {counter}"
            );
        }

        int correctCount = 0;

        Enumerable.Range(0, count)
            .AsParallel()
            .WithDegreeOfParallelism(4)
            .ForAll(x =>
            {
                Interlocked.Increment(ref correctCount);
            });

        Console.WriteLine($"З Interlocked: {correctCount}");
        Console.WriteLine($"очікуваний результат: {count}");
    }
}