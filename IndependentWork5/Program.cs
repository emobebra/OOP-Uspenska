
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

// поліморфізм через Object

class Product
{
    public string Name { get; set; }
    public double Price { get; set; }

    public Product(string name, double price)
    {
        Name = name;
        Price = price;
    }

    public override string ToString()
    {
        return $"товар: {Name}, ціна: {Price} грн";
    }
}

class Customer
{
    public string Name { get; set; }
    public int Age { get; set; }

    public Customer(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public override string ToString()
    {
        return $"покупець: {Name}, вік: {Age}";
    }
}

class Order
{
    public int Number { get; set; }

    public Order(int number)
    {
        Number = number;
    }

    public override string ToString()
    {
        return $"замовлення №{Number}";
    }
}

// поліморфізм через IEnumerable

class MyCustomCollection : IEnumerable<string>
{
    private List<string> items = new List<string>();

    public void Add(string item)
    {
        items.Add(item);
    }

    public IEnumerator<string> GetEnumerator()
    {
        return items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("завдання 1, object");

        List<object> objects = new List<object>
        {
            new Product("гра в стімі", 300),
            new Customer("Емілія", 18),
            new Order(101)
        };

        foreach (object obj in objects)
        {
            Console.WriteLine(obj);
        }

        Console.WriteLine("\nзавдання 2, IEnumerable");

        MyCustomCollection collection = new MyCustomCollection();

        collection.Add("математика");
        collection.Add("програмування");
        collection.Add("англ. мова");

        foreach (string item in collection)
        {
            Console.WriteLine(item);
        }

        Console.WriteLine("\nзавдання 3, stream");

        File.WriteAllBytes("test.txt",
            new byte[] { 10, 20, 30, 40, 50 });

        using (FileStream fileStream = new FileStream(
            "test.txt", FileMode.Open, FileAccess.Read))
        {
            Console.Write("FileStream: ");
            ProcessStream(fileStream);
        }

        using (MemoryStream memoryStream = new MemoryStream(
            new byte[] { 1, 2, 3, 4, 5 }))
        {
            Console.Write("MemoryStream: ");
            ProcessStream(memoryStream);
        }
    }

    static void ProcessStream(Stream stream)
    {
        int value;

        while ((value = stream.ReadByte()) != -1)
        {
            Console.Write(value + " ");
        }

        Console.WriteLine();
    }
}
