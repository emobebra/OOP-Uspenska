# Самостійна робота №5

**Виконала: Успенська Емілія**

**Тема:** Приклади поліморфізму в .NET.

## Результат роботи

<img width="870" height="336" alt="Знімок екрана 2026-10-22 215338" src="https://github.com/user-attachments/assets/94b791ef-6219-4a77-9003-0f2cd8bdb6ed" />

## Прояв поліморфізму в сценаріях

1. базовий клас Object
колекція зберігає об'єкти як базовий тип object, але Console.WriteLine поліморфно викликає перевизначений ToString() для Product, Customer та Order
List<object> objects = new List<object> { new Product("гра в стімі", 300), new Customer("Емілія", 18) };
foreach (object obj in objects)
{
    Console.WriteLine(obj); 
}

2. інтерфейс IEnumerable
завдяки реалізації IEnumerable<string>, компілятор дозволяє використовувати цикл foreach для користувацької колекції, не знаючи про її внутрішню структуру (список List<string>)
class MyCustomCollection : IEnumerable<string> 
MyCustomCollection collection = new MyCustomCollection();
foreach (string item in collection) { Console.WriteLine(item); } 

3. абстрактний клас Stream
один метод ProcessStream може працювати як з файлами, так і з пам'яттю. він покладається на абстракцію Stream
static void ProcessStream(Stream stream) { stream.ReadByte(); } 
ProcessStream(fileStream);   
ProcessStream(memoryStream); 

## Відповіді на контрольні запитання

**1. Як поліморфізм проявляється при роботі з List<object>?**

при виклику методів (наприклад, ToString()) базового типу object спрацьовують їхні перевизначені версії з похідних класів

**2. Що потрібно реалізувати в MyCustomCollection, щоб її можна було перебирати через
foreach?**

потрібно реалізувати інтерфейс IEnumerable (або IEnumerable<T>) та метод GetEnumerator()

**3. Чому метод ProcessStream(Stream stream) є прикладом поліморфної роботи з абстрактним
класом?**

бо він приймає абстрактний базовий клас Stream, але під час виконання працює з конкретними похідними типами (FileStream, MemoryStream), викликаючи їхні власні реалізації методів читання

**4. Які переваги дає єдиний інтерфейс роботи для різних типів потоків і колекцій?**

дає гнучкість, зменшує дублювання коду та дозволяє використовувати ті самі методи для різних джерел даних
