using System;
using System.Runtime.CompilerServices;

namespace lab3v3
{
    public class NetworkStream : IDisposable
    {
        private bool _disposed = false;
        private string _address;
        private bool _isStreamOpen;

        public string Address
        {
            get { return _address; }
        }

        public bool IsStreamOpen
        {
            get { return _isStreamOpen; }
        }

        public NetworkStream(string address)
        {
            _address = address;
            _isStreamOpen = true;
            Console.WriteLine($"потік до {_address} відкрито");
        }

        public void Send(string data)
        {
            if (_isStreamOpen)
            {
                Console.WriteLine($"надіслано на {_address}: {data}");
            }
            else
            {
                Console.WriteLine("потік закрито, надсилання неможливе");
            }
        }

        // disposing = true, якщо викликали з dispose(), і false, якщо це викликав деструктор
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Console.WriteLine("звільняємо керовані ресурси");
                }

                if (_isStreamOpen)
                {
                    Console.WriteLine($"закриваємо потік до {_address}");
                    _isStreamOpen = false;
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);

            // ресурси вже звільнені, деструктор більше не потрібен
            GC.SuppressFinalize(this);
        }

        ~NetworkStream()
        {
            Console.WriteLine("викликано деструктор");
            Dispose(false);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1. через using");
            using (var stream1 = new NetworkStream("192.168.0.1"))
            {
                stream1.Send("тест");
            }

            Console.WriteLine();
            Console.WriteLine("2. явний виклик dispose()");
            var stream2 = new NetworkStream("192.168.0.2");
            stream2.Send("тест");
            stream2.Dispose();
            stream2.Send("тест 2");

            Console.WriteLine();
            Console.WriteLine("3. без dispose(), працює деструктор");
            CreateWithoutDispose();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine();
            Console.WriteLine("кінець програми");
        }

        // об'єкт в окремому методі, щоб на нього не лишилось посилань і gc міг його зібрати
        [MethodImpl(MethodImplOptions.NoInlining)]
        static void CreateWithoutDispose()
        {
            var stream3 = new NetworkStream("192.168.0.3");
            stream3.Send("дані без dispose");
        }
    }
}