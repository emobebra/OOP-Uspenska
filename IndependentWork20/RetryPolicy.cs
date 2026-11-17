using System;
using System.Threading;

public class RetryPolicy
{
    private readonly int _maxRetries;
    private readonly TimeSpan _initialDelay;
    private readonly double _jitterFactor;
    private readonly Random _random = new Random();

    public RetryPolicy(int maxRetries, TimeSpan initialDelay,
        double jitterFactor = 0.2)
    {
        if (maxRetries < 0)
            throw new ArgumentOutOfRangeException(nameof(maxRetries));

        if (initialDelay < TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(initialDelay));

        if (jitterFactor < 0 || jitterFactor > 1)
            throw new ArgumentOutOfRangeException(nameof(jitterFactor));

        _maxRetries = maxRetries;
        _initialDelay = initialDelay;
        _jitterFactor = jitterFactor;
    }

    public T Execute<T>(Func<T> operation,
        Func<Exception, bool>? shouldRetry = null)
    {
        ArgumentNullException.ThrowIfNull(operation);

        for (int attempt = 0; attempt <= _maxRetries; attempt++)
        {
            Console.WriteLine($"спроба №{attempt + 1}");

            try
            {
                T result = operation();
                Console.WriteLine("операцію виконано успішно");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"помилка: {ex.Message}");

                if (shouldRetry != null && !shouldRetry(ex))
                {
                    Console.WriteLine("повторна спроба заборонена");
                    throw;
                }

                if (attempt == _maxRetries)
                {
                    Console.WriteLine(
                        "усі повторні спроби вичерпано");
                    throw;
                }

                double delay = _initialDelay.TotalMilliseconds
                    * Math.Pow(2, attempt)
                    * (1 + _random.NextDouble()
                    * _jitterFactor * 2 - _jitterFactor);

                Console.WriteLine(
                    $"затримка: {delay:F0} мс");

                Thread.Sleep(TimeSpan.FromMilliseconds(delay));
            }
        }

        throw new InvalidOperationException(
            "операцію не вдалося виконати");
    }
}