using System;

namespace lab2v3
{

    public class Student
    {
        private string _name = string.Empty;
        private string _studentId = string.Empty;
        private double _averageMark;

        public string Name
        {
            get => _name;
            set => _name = string.IsNullOrWhiteSpace(value) ? "Unknown" : value;
        }

        public string StudentId
        {
            get => _studentId;
            set => _studentId = string.IsNullOrWhiteSpace(value) ? "N/A" : value;
        }

        public double AverageMark
        {
            get => _averageMark;
            set => _averageMark = (value >= 0 && value <= 100) ? value : 0.0;
        }

        public Student(string name, string studentId, double averageMark)
        {
            Name = name;
            StudentId = studentId;
            AverageMark = averageMark;
        }

        public Student() : this("New Student", "N/A", 0.0) { }

        public string GetStudentCard() => $"{Name}  ID: {StudentId}  Бал: {AverageMark:F1}";

        ~Student()
        {
            Console.WriteLine($"Деструктор: Об'єкт '{_name}' видалено з пам'яті.");
        }
    }

    class Program
    {
        static void Main()
        {
            
            CreateStudents();

            Console.WriteLine("\n запуск Garbage Collector");
            GC.Collect();
            GC.WaitForPendingFinalizers(); 

            Console.WriteLine("завершення програми");
        }

        static void CreateStudents()
        {
            Student student1 = new Student("Проха Роман", "РВ-148578", 20.5);
            Student student2 = new Student("Ніколаєв Максим", "РВ-102938", 91.5);
            Student student3 = new Student("Федчук Ангеліна", "РВ-554433", 65.0);

            Console.WriteLine($"{student1.GetStudentCard()}  {student2.GetStudentCard()}  {student3.GetStudentCard()}");
        }
    }
}
