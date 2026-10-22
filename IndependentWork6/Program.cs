
using System;
using System.Collections.Generic;

// система сповіщень
interface INotifier
{
    void Send(string message);
}

class EmailNotifier : INotifier
{
    public void Send(string message)
    {
        Console.WriteLine($"Email: {message}");
    }
}

class SmsNotifier : INotifier
{
    public void Send(string message)
    {
        Console.WriteLine($"SMS: {message}");
    }
}

class PushNotifier : INotifier
{
    public void Send(string message)
    {
        Console.WriteLine($"Push: {message}");
    }
}

// обробка файлів
abstract class FileProcessor
{
    public string FileName { get; }

    protected FileProcessor(string fileName)
    {
        FileName = fileName;
    }

    public void Open()
    {
        Console.WriteLine($"відкриття файлу: {FileName}");
    }

    public abstract void Process();

    public void Close()
    {
        Console.WriteLine($"закриття файлу: {FileName}");
    }

    public void Run()
    {
        Open();
        Process();
        Close();
    }
}

class TextFileProcessor : FileProcessor
{
    public TextFileProcessor(string fileName) : base(fileName) { }

    public override void Process()
    {
        Console.WriteLine("обробка текстового файлу");
    }
}

class XmlFileProcessor : FileProcessor
{
    public XmlFileProcessor(string fileName) : base(fileName) { }

    public override void Process()
    {
        Console.WriteLine("обробка XML-файлу");
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("система сповіщень:");

        List<INotifier> notifiers = new List<INotifier>
        {
            new EmailNotifier(),
            new SmsNotifier(),
            new PushNotifier()
        };

        foreach (INotifier notifier in notifiers)
        {
            notifier.Send("привіт!");
        }

        Console.WriteLine("\nобробка файлів:");

        List<FileProcessor> processors = new List<FileProcessor>
        {
            new TextFileProcessor("document.txt"),
            new XmlFileProcessor("data.xml")
        };

        foreach (FileProcessor processor in processors)
        {
            processor.Run();
        }
    }
}
