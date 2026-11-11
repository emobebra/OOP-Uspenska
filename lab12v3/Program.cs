
using System;
using System.IO;
using System.Net.Http;

FileProcessor fileProcessor = new FileProcessor();
NetworkClient networkClient = new NetworkClient();

TimeSpan delay = TimeSpan.FromMilliseconds(200);

Console.WriteLine("робота з файлом");

try
{
    var lines = RetryHelper.ExecuteWithRetry(
        () => fileProcessor.GetLines("data.txt"),
        retryCount: 3,
        initialDelay: delay,
        shouldRetry: ex => ex is IOException
    );

    Console.WriteLine("вміст файлу:");

    foreach (string line in lines)
    {
        Console.WriteLine(line);
    }
}
catch (Exception ex)
{
    Console.WriteLine($"кінцева помилка: {ex.Message}");
}

Console.WriteLine("\n мережевий запит");

try
{
    string response = RetryHelper.ExecuteWithRetry(
        () => networkClient.GetApiResponse("https://example.com/api"),
        retryCount: 4,
        initialDelay: delay,
        shouldRetry: ex => ex is HttpRequestException
    );

    Console.WriteLine(response);
}
catch (Exception ex)
{
    Console.WriteLine($"кінцева помилка: {ex.Message}");
}

Console.WriteLine("\n перевірка shouldRetry");

try
{
    RetryHelper.ExecuteWithRetry<string>(
        () => throw new ArgumentException("неправильний аргумент"),
        retryCount: 3,
        initialDelay: delay,
        shouldRetry: ex => ex is IOException ||
                           ex is HttpRequestException
    );
}
catch (ArgumentException ex)
{
    Console.WriteLine($"помилку не повторено: {ex.Message}");
}

Console.WriteLine("\n програму завершено");
