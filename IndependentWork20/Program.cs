using System;

Console.OutputEncoding = System.Text.Encoding.UTF8;

RetryPolicy policy = new RetryPolicy(
    maxRetries: 3,
    initialDelay: TimeSpan.FromMilliseconds(500),
    jitterFactor: 0.2
);

Console.WriteLine("сценарій 1, успіх після помилок");

int successAttempts = 0;

try
{
    string result = policy.Execute(() =>
    {
        successAttempts++;

        if (successAttempts < 3)
        {
            throw new InvalidOperationException(
                "тимчасова помилка з'єднання");
        }

        return "дані отримано успішно";
    },
    ex => ex is InvalidOperationException);

    Console.WriteLine($"результат: {result}");
}
catch (Exception ex)
{
    Console.WriteLine($"остаточна помилка: {ex.Message}");
}

Console.WriteLine();
Console.WriteLine("сценарій 2, постійна невдача");

try
{
    string result = policy.Execute<string>(() =>
    {
        throw new InvalidOperationException(
            "сервер не відповідає");
    },
    ex => ex is InvalidOperationException);

    Console.WriteLine(result);
}
catch (Exception ex)
{
    Console.WriteLine($"остаточна помилка: {ex.Message}");
}

Console.WriteLine();
Console.WriteLine("сценарій 3, заборона повтору");

try
{
    policy.Execute<string>(() =>
    {
        throw new ArgumentException(
            "некоректні вхідні дані");
    },
    ex => ex is InvalidOperationException);
}
catch (Exception ex)
{
    Console.WriteLine($"операцію зупинено: {ex.Message}");
}