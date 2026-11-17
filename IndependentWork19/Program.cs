using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("\n1. власний тип винятку");

try
{
    CreateTicket(0);
}
catch (TicketException ex)
{
    Console.WriteLine($"помилка: {ex.Message}");
    Console.WriteLine($"номер звернення: {ex.TicketId}");
    Console.WriteLine($"код помилки: {ex.ErrorCode}");
}
finally
{
    Console.WriteLine("перевірку звернення завершено");
}

// сценарій 1, retry

Console.WriteLine("\n2. retry, підключення до API");

int apiAttempts = 0;

var retryPipeline = new ResiliencePipelineBuilder()
    .AddRetry(new RetryStrategyOptions
    {
        ShouldHandle = new PredicateBuilder()
            .Handle<HttpRequestException>(),
        MaxRetryAttempts = 3,
        Delay = TimeSpan.FromMilliseconds(300),
        BackoffType = DelayBackoffType.Exponential,
        UseJitter = true,
        OnRetry = args =>
        {
            Console.WriteLine(
                $"повторна спроба через {args.RetryDelay.TotalMilliseconds:F0} мс");
            return default;
        }
    })
    .Build();

try
{
    retryPipeline.Execute(() =>
    {
        apiAttempts++;
        Console.WriteLine($"спроба {apiAttempts}: запит до API");

        if (apiAttempts < 3)
        {
            throw new HttpRequestException(
                "API тимчасово недоступний");
        }

        Console.WriteLine("успіх: дані отримано");
    });
}
catch (HttpRequestException ex)
{
    Console.WriteLine($"остаточна помилка: {ex.Message}");
}

// сценарій 2, circuit breaker

Console.WriteLine("\n3. circuit breaker, база даних");

var breakerPipeline = new ResiliencePipelineBuilder()
    .AddCircuitBreaker(new CircuitBreakerStrategyOptions
    {
        ShouldHandle = new PredicateBuilder()
            .Handle<InvalidOperationException>(),
        FailureRatio = 0.5,
        MinimumThroughput = 2,
        SamplingDuration = TimeSpan.FromSeconds(10),
        BreakDuration = TimeSpan.FromSeconds(3),
        OnOpened = args =>
        {
            Console.WriteLine("circuit breaker відкрито");
            return default;
        },
        OnClosed = args =>
        {
            Console.WriteLine("circuit breaker закрито");
            return default;
        }
    })
    .Build();

for (int i = 1; i <= 4; i++)
{
    try
    {
        int attempt = i;

        breakerPipeline.Execute(() =>
        {
            Console.WriteLine($"запит {attempt}: підключення до БД");

            throw new InvalidOperationException(
                "База даних недоступна");
        });
    }
    catch (BrokenCircuitException)
    {
        Console.WriteLine(
            $"запит {i} заблоковано circuit breaker");
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"помилка: {ex.Message}");
    }
}

// сценарій 3, timeout

Console.WriteLine("\n4. timeout, надсилання повідомлення");

var timeoutPipeline = new ResiliencePipelineBuilder()
    .AddTimeout(TimeSpan.FromMilliseconds(500))
    .Build();

try
{
    await timeoutPipeline.ExecuteAsync(async token =>
    {
        Console.WriteLine("надсилання повідомлення...");

        await Task.Delay(1500, token);

        Console.WriteLine("повідомлення надіслано");
    });
}
catch (TimeoutRejectedException)
{
    Console.WriteLine(
        "час очікування вичерпано. операцію скасовано");
}

Console.WriteLine("\nусі сценарії завершено");

static void CreateTicket(int ticketId)
{
    if (ticketId <= 0)
    {
        throw new TicketException(
            "некоректний номер звернення",
            ticketId,
            "INVALID_TICKET_ID");
    }

    Console.WriteLine($"звернення {ticketId} створено");
}

public class TicketException : Exception
{
    public int TicketId { get; }
    public string ErrorCode { get; } = "";

    public TicketException() { }

    public TicketException(string message)
        : base(message) { }

    public TicketException(string message, Exception innerException)
        : base(message, innerException) { }

    public TicketException(
        string message,
        int ticketId,
        string errorCode)
        : base(message)
    {
        TicketId = ticketId;
        ErrorCode = errorCode;
    }
}