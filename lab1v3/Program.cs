using System;

namespace lab1v3

{
    public class Student
    {
        private string name;
        private int id;
        private double averageMark;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public double AverageMark
        {
            get { return averageMark; }
            set 
            { 
                if (value >= 0 && value <= 100)
                    averageMark = value; 
                else
                    averageMark = 0;
            }
        }

        public Student(string name, int id, double averageMark)
        {
            this.name = name;
            this.id = id;
            AverageMark = averageMark; 
        }


        public void PrintCard()
        {
            Console.WriteLine(" студентський квиток ");
            Console.WriteLine($" піб студента : {name}");
            Console.WriteLine($" айді Квитка    : {id}");
            Console.WriteLine($" середній бал : {averageMark:F1}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Student st1 = new Student("Федчук Ангеліна", 101, 92.5);
            Student st2 = new Student("Ніколаєв Максим", 102, 85.0);
            Student st3 = new Student("Бзіта Олена", 103, 78.4);

            st1.PrintCard();
            st2.PrintCard();
            st3.PrintCard();
        }
    }
}