using System;

namespace lab4v3
{
    // базовий клас
    class Vehicle
    {
        private string brand;
        private int year;

        public string Brand
        {
            get { return brand; }
            set { brand = value; }
        }

        public int Year
        {
            get { return year; }
            set { year = value; }
        }

        public Vehicle(string brand, int year)
        {
            this.brand = brand;
            this.year = year;
        }

        // virtual дозволяє похідним класам перевизначити метод
        public virtual void Drive()
        {
            Console.WriteLine($"{Brand} ({Year}) їде");
        }

        // не virtual: у похідному класі його буде приховано через new
        public string GetVehicleType()
        {
            return "транспортний засіб";
        }
    }

    class Car : Vehicle
    {
        private int numDoors;

        public int NumDoors
        {
            get { return numDoors; }
            set { numDoors = value; }
        }

        // base() викликає конструктор базового класу
        public Car(string brand, int year, int numDoors) : base(brand, year)
        {
            this.numDoors = numDoors;
        }

        // override замінює реалізацію virtual методу
        public override void Drive()
        {
            Console.WriteLine($"автомобіль {Brand} ({Year}) їде, дверей: {NumDoors}");
        }

        public void OpenTrunk()
        {
            Console.WriteLine($"багажник автомобіля {Brand} відкрито");
        }

        // new приховує метод базового класу, а не перевизначає його
        public new string GetVehicleType()
        {
            return "легковий автомобіль";
        }
    }

    class Motorcycle : Vehicle
    {
        private bool hasSidecar;

        public bool HasSidecar
        {
            get { return hasSidecar; }
            set { hasSidecar = value; }
        }

        public Motorcycle(string brand, int year, bool hasSidecar) : base(brand, year)
        {
            this.hasSidecar = hasSidecar;
        }

        public override void Drive()
        {
            string sidecar = HasSidecar ? "з коляскою" : "без коляски";
            Console.WriteLine($"мотоцикл {Brand} ({Year}) їде {sidecar}");
        }

        public void Wheelie()
        {
            Console.WriteLine($"мотоцикл {Brand} піднявся на заднє колесо");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Vehicle vehicle = new Vehicle("bus", 2000);
            Car car = new Car("chevrolet", 2020, 4);
            Motorcycle moto = new Motorcycle("kawasaki", 2018, false);

            Console.WriteLine("створені об'єкти:");
            vehicle.Drive();
            car.Drive();
            moto.Drive();

            // поліморфізм - посилання типу vehicle, але викликається метод реального об'єкта
            Console.WriteLine("\nполіморфізм (посилання базового класу):");
            Vehicle[] vehicles = { vehicle, car, moto };
            foreach (Vehicle v in vehicles)
            {
                v.Drive();
            }

            Console.WriteLine("\nвласні методи похідних класів:");
            car.OpenTrunk();
            moto.Wheelie();

            // override проти new - при new результат залежить від типу посилання
            Console.WriteLine("\nOverride проти new:");
            Vehicle carAsVehicle = car;
            Console.WriteLine("Drive через Vehicle (override): ");
            carAsVehicle.Drive();
            Console.WriteLine("GetVehicleType через Vehicle (new): " + carAsVehicle.GetVehicleType());
            Console.WriteLine("GetVehicleType через Car (new): " + car.GetVehicleType());
        }
    }
}