using System;
using System.IO;

public class InvalidFileFormatException : Exception
{
    public string FileName { get; }
    public string ExpectedFormat { get; }

    public InvalidFileFormatException()
        : base("неправильний формат файлу")
    {
        FileName = "";
        ExpectedFormat = "";
    }

    public InvalidFileFormatException(string message)
        : base(message)
    {
        FileName = "";
        ExpectedFormat = "";
    }

    public InvalidFileFormatException(string message, Exception innerException)
        : base(message, innerException)
    {
        FileName = "";
        ExpectedFormat = "";
    }

    public InvalidFileFormatException(string fileName, string expectedFormat)
        : base($"файл '{fileName}' повинен мати формат {expectedFormat}")
    {
        FileName = fileName;
        ExpectedFormat = expectedFormat;
    }
}

public class Result<T>
{
    public bool IsSuccess { get; }
    public T Value { get; }
    public string Error { get; }

    private Result(T value, bool isSuccess, string error)
    {
        Value = value;
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(value, true, "");
    }

    public static Result<T> Failure(string error)
    {
        return new Result<T>(default!, false, error);
    }

    public static Result<T> Failure(Exception ex)
    {
        return Failure(ex.Message);
    }
}

public class FileProcessorService
{
    public string ReadFile(string path)
    {
        if (Path.GetExtension(path).ToLowerInvariant() != ".txt")
        {
            throw new InvalidFileFormatException(
                Path.GetFileName(path), ".txt");
        }

        return File.ReadAllText(path);
    }

    public Result<bool> WriteToFile(string path, string content)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return Result<bool>.Failure("шлях до файлу невірний");
        }

        if (content == null)
        {
            return Result<bool>.Failure("вміст файлу не може бути null");
        }

        try
        {
            File.WriteAllText(path, content);
            return Result<bool>.Success(true);
        }
        catch (Exception ex) when (
            ex is UnauthorizedAccessException ||
            ex is IOException ||
            ex is ArgumentException ||
            ex is NotSupportedException)
        {
            return Result<bool>.Failure(ex);
        }
    }
}

class Program
{
    static void Main()
    {
        FileProcessorService service = new FileProcessorService();

        Console.WriteLine("Варіант 3: Обробка файлів\n");

        File.WriteAllText("test.txt", "тестовий текстовий файл");
        File.WriteAllText("test.csv", "name,age");

        // сценарій 1, успішне читання

        Console.WriteLine("1. читання правильного файлу:");

        try
        {
            string text = service.ReadFile("test.txt");
            Console.WriteLine("успішно: " + text);
        }
        catch (InvalidFileFormatException ex)
        {
            Console.WriteLine("помилка: " + ex.Message);
        }

        // сценарій 2, власний виняток

        Console.WriteLine("\n2. читання файлу неправильного формату:");

        try
        {
            string text = service.ReadFile("test.csv");
            Console.WriteLine(text);
        }
        catch (InvalidFileFormatException ex)
        {
            Console.WriteLine("власний виняток: " + ex.Message);
            Console.WriteLine("файл: " + ex.FileName);
            Console.WriteLine("очікуваний формат: " + ex.ExpectedFormat);
        }

        // сценарій 3, успішний запис

        Console.WriteLine("\n3. успішний запис файлу:");

        Result<bool> result1 = service.WriteToFile(
            "output.txt", "запис виконано успішно");

        if (result1.IsSuccess)
        {
            Console.WriteLine("успішно: файл output.txt створено");
        }
        else
        {
            Console.WriteLine("помилка: " + result1.Error);
        }

        // сценарй 4, помилка запису

        Console.WriteLine("\n4. запис із невалідним шляхом:");

        Result<bool> result2 = service.WriteToFile(
            "", "тестовий запис");

        if (result2.IsSuccess)
        {
            Console.WriteLine("успігно: дані записано");
        }
        else
        {
            Console.WriteLine("Result.Failure: " + result2.Error);
        }
    }
}