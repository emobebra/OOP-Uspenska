using System.Diagnostics;
using System.Threading;

class Program
{
    static void Main()
    {
        Console.WriteLine("Independent work 12: LINQ vs PLINQ");
        Console.WriteLine($"Processors: {Environment.ProcessorCount}");

        var random = new Random(12);
        var numbers = new List<int>(10_000_000);
        for (int i = 0; i < 10_000_000; i++)
            numbers.Add(random.Next(1000, 100000));

        int[] sizes = { 1_000_000, 5_000_000, 10_000_000 };
        Func<int, double>[] operations = { SimpleCalculation, ComplexCalculation };
        string[] names = { "Simple calculation", "Complex calculation" };

        // Прогрів перед вимірюваннями
        _ = numbers.Take(100).Where(x => x % 3 == 0).Select(ComplexCalculation).Sum();
        _ = numbers.Take(100).AsParallel().Where(x => x % 3 == 0).Select(ComplexCalculation).Sum();

        for (int operation = 0; operation < operations.Length; operation++)
        {
            Console.WriteLine($"\n{names[operation]}");
            Console.WriteLine("Size | LINQ (ms) | PLINQ (ms) | Results match");

            foreach (int size in sizes)
            {
                // Take використовується без копіювання великої частини списку
                var data = numbers.Take(size).ToArray();
                var calculate = operations[operation];

                var sw = Stopwatch.StartNew();
                double linq = data.Where(x => x % 3 == 0).Select(calculate).Sum();
                sw.Stop();
                double linqMs = sw.Elapsed.TotalMilliseconds;

                sw.Restart();
                double plinq = data.AsParallel().Where(x => x % 3 == 0)
                    .Select(calculate).Sum();
                sw.Stop();
                double plinqMs = sw.Elapsed.TotalMilliseconds;

                bool matches = Math.Abs(linq - plinq) < 0.000001 * Math.Max(1, Math.Abs(linq));
                Console.WriteLine($"{size} | {linqMs:F2} | {plinqMs:F2} | {matches}");
            }
        }

        Console.WriteLine("\nThread safety demonstration:");
        const int count = 200_000;
        for (int attempt = 1; attempt <= 3; attempt++)
        {
            int unsafeCounter = 0;
            Enumerable.Range(0, count).AsParallel()
                .WithDegreeOfParallelism(4)
                .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                .Select(x =>
                {
                    int value = unsafeCounter;
                    Thread.SpinWait(30);
                    unsafeCounter = value + 1;
                    return x;
                }).ToArray();

            int safeCounter = 0;
            Enumerable.Range(0, count).AsParallel()
                .WithDegreeOfParallelism(4)
                .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                .Select(x =>
                {
                    Interlocked.Increment(ref safeCounter);
                    return x;
                }).ToArray();

            Console.WriteLine($"Attempt {attempt}: expected {count}, unsafe {unsafeCounter}, safe {safeCounter}");
        }
    }

    static double SimpleCalculation(int number) => Math.Sqrt(number);

    static double ComplexCalculation(int number)
    {
        double value = number;
        for (int i = 0; i < 12; i++)
            value = Math.Sqrt(value + i + 1) + Math.Sin(value);
        return value;
    }
}
