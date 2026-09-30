using System;
using System.Collections.Generic;

class Employee
{
    public string Name { get; set; }

    public Employee(string name)
    {
        Name = name;
    }

    // у базовому класі просто заглушка, кожен нащадок рахує по-своєму
    public virtual decimal CalculatePay()
    {
        return 0;
    }
}

class HourlyEmployee : Employee
{
    public double HoursWorked { get; set; }
    public decimal HourlyRate { get; set; }

    public HourlyEmployee(string name, double hoursWorked, decimal hourlyRate) : base(name)
    {
        HoursWorked = hoursWorked;
        HourlyRate = hourlyRate;
    }

    public override decimal CalculatePay()
    {
        return (decimal)HoursWorked * HourlyRate;
    }
}

class SalariedEmployee : Employee
{
    public decimal WeeklySalary { get; set; }

    public SalariedEmployee(string name, decimal weeklySalary) : base(name)
    {
        WeeklySalary = weeklySalary;
    }

    public override decimal CalculatePay()
    {
        return WeeklySalary;
    }
}

class CommissionEmployee : Employee
{
    public decimal SalesAmount { get; set; }
    public decimal CommissionRate { get; set; }

    public CommissionEmployee(string name, decimal salesAmount, decimal commissionRate) : base(name)
    {
        SalesAmount = salesAmount;
        CommissionRate = commissionRate;
    }

    public override decimal CalculatePay()
    {
        return SalesAmount * CommissionRate;
    }
}

class Program
{
    static void Main()
    {
        List<Employee> employees = new List<Employee>();
        employees.Add(new HourlyEmployee("Іван", 40, 150m));
        employees.Add(new SalariedEmployee("Марія", 8000m));
        employees.Add(new CommissionEmployee("Олег", 50000m, 0.1m));

        decimal total = 0;

        // тут спрацьовує поліморфізм - тип змінної Employee, а метод бере від реального класу
        foreach (Employee e in employees)
        {
            decimal pay = e.CalculatePay();
            Console.WriteLine($"{e.Name} ({e.GetType().Name}): {pay}");
            total += pay;
        }

        Console.WriteLine($"загальна сума виплат: {total}");
    }
}