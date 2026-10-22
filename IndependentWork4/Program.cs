using System;

namespace IndependentWork4
{
    public class LibraryItem
    {
        private string _title;
        private int _publicationYear;
        private string _uniqueId;

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public int PublicationYear
        {
            get { return _publicationYear; }
            set { _publicationYear = value; }
        }

        public string UniqueId
        {
            get { return _uniqueId; }
        }

        public LibraryItem(string title, int publicationYear, string uniqueId)
        {
            _title = title;
            _publicationYear = publicationYear;
            _uniqueId = uniqueId;
        }

        public virtual void DisplayDetails()
        {
            Console.WriteLine($"ID: {_uniqueId}, назва: {_title}, рік видання: {_publicationYear}");
        }
    }

    public class Book : LibraryItem
    {
        private string _author;

        public string Author
        {
            get { return _author; }
            set { _author = value; }
        }

        public Book(string title, int publicationYear, string uniqueId, string author)
            : base(title, publicationYear, uniqueId)
        {
            _author = author;
        }

        public override void DisplayDetails()
        {
            base.DisplayDetails();
            Console.WriteLine($"Автор: {_author}");
        }
    }

    public class Journal : LibraryItem
    {
        private int _issueNumber;

        public int IssueNumber
        {
            get { return _issueNumber; }
            set { _issueNumber = value; }
        }

        public Journal(string title, int publicationYear, string uniqueId, int issueNumber)
            : base(title, publicationYear, uniqueId)
        {
            _issueNumber = issueNumber;
        }

        public override void DisplayDetails()
        {
            base.DisplayDetails();
            Console.WriteLine($"Номер випуску: {_issueNumber}");
        }
    }

    public class AudioBook : Book
    {
        private string _narrator;

        public string Narrator
        {
            get { return _narrator; }
            set { _narrator = value; }
        }

        public AudioBook(string title, int publicationYear, string uniqueId, string author, string narrator)
            : base(title, publicationYear, uniqueId, author)
        {
            _narrator = narrator;
        }

        public override void DisplayDetails()
        {
            base.DisplayDetails();
            Console.WriteLine($"Оповідач: {_narrator}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            LibraryItem[] items =
            {
                new Book("Кобзар", 1840, "B-001", "Тарас Шевченко"),
                new Journal("Наука і життя", 2020, "J-001", 5),
                new AudioBook("Тіні забутих предків", 1911, "A-001",
                    "Михайло Коцюбинський", "Богдан Ступка")
            };

            foreach (LibraryItem item in items)
            {
                item.DisplayDetails();
                Console.WriteLine();
            }
        }
    }
}