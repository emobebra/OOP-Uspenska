
using System;
using System.Collections.Generic;

class PrintManager
{
    private HashSet<string> users = new HashSet<string>();
    private Queue<string> printQueue = new Queue<string>();
    private Stack<string> printHistory = new Stack<string>();

    public void AddUser(string user)
    {
        if (users.Add(user))
            Console.WriteLine($"додано користувача: {user}");
        else
            Console.WriteLine($"користувач {user} вже існує");
    }

    public void CheckUser(string user)
    {
        Console.WriteLine($"користувач {user} існує: {users.Contains(user)}");
    }

    public void RemoveUser(string user)
    {
        if (users.Remove(user))
            Console.WriteLine($"видалено користувача: {user}");
    }

    public void AddDocument(string document)
    {
        printQueue.Enqueue(document);
        Console.WriteLine($"додано в чергу: {document}");
    }

    public void ShowNextDocument()
    {
        if (printQueue.Count > 0)
            Console.WriteLine($"наступний документ: {printQueue.Peek()}");
    }

    public void PrintDocument()
    {
        if (printQueue.Count > 0)
        {
            string document = printQueue.Dequeue();
            printHistory.Push(document);
            Console.WriteLine($"надруковано: {document}");
        }
    }

    public void ShowLastPrinted()
    {
        if (printHistory.Count > 0)
            Console.WriteLine($"останній надрукований: {printHistory.Peek()}");
    }

    public void RemoveFromHistory()
    {
        if (printHistory.Count > 0)
        {
            string document = printHistory.Pop();
            Console.WriteLine($"видалено з історії: {document}");
        }
    }
}

class Program
{
    static void Main()
    {
        PrintManager manager = new PrintManager();

        Console.WriteLine("HashSet");
        manager.AddUser("Емілія");
        manager.AddUser("Роман");
        manager.AddUser("Емілія");
        manager.CheckUser("Роман");
        manager.RemoveUser("Роман");

        Console.WriteLine("\n Queue");
        manager.AddDocument("звіт.pdf");
        manager.AddDocument("лабораторна.docx");
        manager.AddDocument("реферат.pdf");
        manager.ShowNextDocument();
        manager.PrintDocument();
        manager.PrintDocument();
        manager.ShowNextDocument();

        Console.WriteLine("\n Stack");
        manager.ShowLastPrinted();
        manager.RemoveFromHistory();
        manager.ShowLastPrinted();
    }
}
