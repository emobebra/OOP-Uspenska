
using System;
using System.Collections.Generic;
using System.IO;

public class FileProcessor
{
    private int attempts = 0;

    public List<string> GetLines(string path)
    {
        attempts++;

        if (attempts <= 2)
        {
            throw new IOException("тимчасова помилка читання файлу");
        }

        return new List<string>
        {
            "перший рядок файлу",
            "другий рядок файлу",
            "третій рядок файлу"
        };
    }
}
