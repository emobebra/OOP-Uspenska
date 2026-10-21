namespace Lab8v3;

public class Product
{
    public int Id { get; }
    public string Name { get; }
    public decimal Price { get; }
    public string Category { get; }

    public Product(int id, string name, decimal price, string category)
    {
        Id = id;
        Name = name;
        Price = price;
        Category = category;
    }

    public override string ToString() => $"{Name} ({Category}) - {Price} грн";
}