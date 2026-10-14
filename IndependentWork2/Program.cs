public class Program
{
    public static void Main(string[] args)
    {

        Console.WriteLine("створення товарів");

        Product laptop = new Product(101, "laptop", 35000m, "electronics", 15);
        Console.WriteLine($"товар 1 (основний конструктор): {laptop}");

        Product mouse = new Product(102, "mouse", 800m);
        Console.WriteLine($"товар 2 (скорочений конструктор): {mouse}");

        Product laptopCopy = new Product(laptop);
        Console.WriteLine($"товар 3 (конструктор копіювання): {laptopCopy}");
    }
}