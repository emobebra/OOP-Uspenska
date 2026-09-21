using System;
using System.Runtime.CompilerServices;

namespace lab3v3
{
    // Клас імітує мережевий потік. Реалізує IDisposable, щоб "потік" можна було закрити
    public class NetworkStream : IDisposable
    {
        // Чи вже викликали звільнення ресурсів (щоб не звільняти двічі)
        private bool _disposed = false;

        // Адреса, до якої "підключились"
        private string _address;

        // Імітація некерованого ресурсу: true - потік відкритий
        private bool _isStreamOpen;

        // Публічні властивості тільки для читання
        public string Address
        {
            get { return _address; }
        }

        public bool IsStreamOpen
        {
            get { return _isStreamOpen; }
        }

        // Конструктор "відкриває" потік
        public NetworkStream(string address)
        {
            _address = address;
            _isStreamOpen = true;
            Console.WriteLine($"Потік до {_address} відкрито");
        }

        // Надсилання даних працює тільки поки потік відкритий
        public void Send(string data)
        {
            if (_isStreamOpen)
            {
                Console.WriteLine($"Надіслано на {_address}: {data}");
            }
            else
            {
                Console.WriteLine("Потік закрито, надсилання неможливе");
            }
        }

        // Головний метод звільнення. disposing = true, якщо викликали з Dispose(),
        // і false, якщо це викликав деструктор (тоді керовані об'єкти чіпати не можна)
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Тут звільняються керовані ресурси
                    Console.WriteLine("Звільняємо керовані ресурси");
                }

                // Некерований ресурс звільняємо в будь-якому випадку
                if (_isStreamOpen)
                {
                    Console.WriteLine($"Закриваємо потік до {_address}");
                    _isStreamOpen = false;
                }

                _disposed = true;
            }
        }

        // Публічний метод, який викликає програміст (або using)
        public void Dispose()
        {
            Dispose(true);

            // Ресурси вже звільнені, тому деструктор більше не потрібен
            GC.SuppressFinalize(this);
        }

        // Деструктор - запасний варіант, якщо забули викликати Dispose()
        ~NetworkStream()
        {
            Console.WriteLine("Викликано деструктор");
            Dispose(false);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- 1. Через using ---");
            using (var stream1 = new NetworkStream("192.168.0.1"))
            {
                stream1.Send("Привіт");
            }
            // Dispose() викликається автоматично при виході з блоку

            Console.WriteLine();
            Console.WriteLine("--- 2. Явний виклик Dispose() ---");
            var stream2 = new NetworkStream("192.168.0.2");
            stream2.Send("Тестове повідомлення");
            stream2.Dispose();
            stream2.Send("Ще одне повідомлення");

            Console.WriteLine();
            Console.WriteLine("--- 3. Без Dispose(), працює деструктор ---");
            CreateWithoutDispose();

            // Примусово запускаємо збирач сміття і чекаємо, поки відпрацюють деструктори
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine();
            Console.WriteLine("Кінець програми");
        }

        // Об'єкт створюється в окремому методі, щоб після виходу з нього
        // на нього не лишилось посилань і GC міг його зібрати
        [MethodImpl(MethodImplOptions.NoInlining)]
        static void CreateWithoutDispose()
        {
            var stream3 = new NetworkStream("192.168.0.3");
            stream3.Send("Дані без Dispose");
        }
    }
}