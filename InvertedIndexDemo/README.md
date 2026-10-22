# Самостійна робота №10

**Тема:** Колекції в практиці ООП: інвертований індекс і вимірювання продуктивності.

Консольна програма на C# створює інвертований індекс документів і підтримує пошук за одним словом, пошук AND (усі слова) і OR (будь-яке слово). Порівнюються лінійний пошук і пошук через індекс на 10, 100, 1000 і 10000 документах.

## Запуск

```bash
dotnet run -c Release
```

## Приклад виводу

```text
Loaded 5 documents
Index contains ... unique words

Performance test (average ticks per search):
  Docs |   Linear ticks |    Index ticks | Same results
    10 |      ...       |       ...      | True
   100 |      ...       |       ...      | True
  1000 |      ...       |       ...      | True
 10000 |      ...       |       ...      | True

Enter search query (or 'exit'): c# async

--- Search Results (AND) ---
[4] C# Advanced Features
  C# supports LINQ, async/await, and many modern programming paradigms.
Found 1 documents

--- Search Results (OR) ---
[1] Introduction to C#
[4] C# Advanced Features
[5] Comparing Languages
Found 3 documents
```

Час залежить від комп'ютера. Замість `...` програма виведе справжні значення.

Детальніше — у [REPORT.md](REPORT.md).
