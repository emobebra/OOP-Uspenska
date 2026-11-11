
using System;
using System.Threading;

public static class RetryHelper
{
    public static T ExecuteWithRetry<T>(
        Func<T> operation,
        int retryCount = 3,
        TimeSpan initialDelay = default,
        Func<Exception, bool>? shouldRetry = null)
    {
        if (retryCount < 0)
            throw new ArgumentOutOfRangeException(nameof(retryCount));

        if (initialDelay == default)
            initialDelay = TimeSpan.FromMilliseconds(200);

        if (initialDelay < TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(initialDelay));

        for (int attempt = 0; ; attempt++)
        {
            try
            {
                Console.WriteLine($"спроба №{attempt + 1}");

                T result = operation();

                Console.WriteLine("операцію виконано успішно!");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"помилка: {ex.Message}");

                if (attempt >= retryCount ||
                    (shouldRetry != null && !shouldRetry(ex)))
                {
                    Console.WriteLine("повторні спроби припинено");
                    throw;
                }

                double delay = initialDelay.TotalMilliseconds *
                               Math.Pow(2, attempt);

                Console.WriteLine($"затримка: {delay} мс");
                Thread.Sleep(TimeSpan.FromMilliseconds(delay));
            }
            finally
            {
                Console.WriteLine("спробу завершено.\n");
            }
        }
    }
}
