
using System;
using System.Collections.Generic;
using System.Linq;

class Subject
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

class Grade
{
    public Subject Subject { get; set; } = new Subject();
    public int Score { get; set; }
}

class Student
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public List<Grade> Grades { get; set; } = new();
}

static class GradeExtensions
{
    public static double AverageScore(this IEnumerable<Grade> grades)
    {
        return grades.Any() ? grades.Average(g => g.Score) : 0;
    }
}

class Program
{
    static void Main()
    {
        var math = new Subject { Id = 1, Name = "Математика" };
        var programming = new Subject { Id = 2, Name = "Програмування" };
        var english = new Subject { Id = 3, Name = "Англійська мова" };

        var students = new List<Student>();

        string[] names =
        {
            "Анна", "Максим", "Олена", "Іван",
            "Марія", "Андрій", "Софія", "Дмитро",
            "Катерина", "Олександр", "Юлія", "Богдан"
        };

        for (int i = 0; i < names.Length; i++)
        {
            students.Add(new Student
            {
                Id = i + 1,
                Name = names[i],
                Grades = new List<Grade>
                {
                    new Grade { Subject = math, Score = 7 + i % 6 },
                    new Grade { Subject = programming, Score = 8 + i % 5 },
                    new Grade { Subject = english, Score = 6 + i % 7 }
                }
            });
        }

        // завдання 1, SelectMany
        Console.WriteLine(" SELECTMANY");

        var allGrades = students
            .SelectMany(s => s.Grades.Select(g => new
            {
                StudentName = s.Name,
                SubjectName = g.Subject.Name,
                Score = g.Score
            }))
            .ToList();

        foreach (var grade in allGrades)
        {
            Console.WriteLine(
                $"{grade.StudentName} | {grade.SubjectName} | {grade.Score}");
        }

        Console.WriteLine($"Усього оцінок: {allGrades.Count}");

        // завдання 2, пагінація
        Console.WriteLine("\n пагінація ");

        int pageSize = 5;
        int totalPages = (int)Math.Ceiling(
            (double)students.Count / pageSize);

        for (int page = 1; page <= totalPages; page++)
        {
            Console.WriteLine($"\nСторінка {page} з {totalPages}");

            var pageStudents = students
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            foreach (var student in pageStudents)
            {
                Console.WriteLine(
                    $"{student.Id}. {student.Name}");
            }
        }

        // завдання 3, власний метод розширення
        Console.WriteLine("\n AVERAGESCORE");

        foreach (var student in students)
        {
            double average = student.Grades.AverageScore();

            Console.WriteLine(
                $"{student.Name}: середній бал = {average:F2}");
        }

        var gradesOnly = students.SelectMany(s => s.Grades);

        Console.WriteLine(
            $"\nЗагальний середній бал: {gradesOnly.AverageScore():F2}");
    }
}
