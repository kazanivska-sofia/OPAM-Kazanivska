using System;
using System.IO;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        int N = 11;
        int K = 22;

        Console.WriteLine($"=== ЛАБОРАТОРНА РОБОТА №3 (N={N}, K={K}) ===");

        Console.WriteLine("\n--- Завдання 1: Обхід каталогів (.cs файли та їх розмір) ---");
        Console.Write("Введіть шлях до папки (або натисніть Enter для поточної папки): ");
        string path = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(path))
        {
            path = Directory.GetCurrentDirectory();
        }

        if (!Directory.Exists(path))
        {
            Console.WriteLine($"Помилка: Папка '{path}' не існує!");
        }
        else
        {
            var (count, totalSize) = Task1_DirectorySearch(new DirectoryInfo(path));
            Console.WriteLine($"Знайдено .cs файлів: {count}");
            Console.WriteLine($"Сумарний розмір: {totalSize} байт");
        }

        Console.WriteLine("\n--- Завдання 2: Поділ навпіл (чи всі елементи однакові) ---");
        int[] arr2 = GenerateArray(16, -20, 20, N);
        Console.WriteLine("Масив: [" + string.Join(", ", arr2) + "]");
        
        int maxDepth = 0;
        bool allEqualRec = Task2_AllEqual(arr2, 0, arr2.Length - 1, 1, ref maxDepth);
        Console.WriteLine($"Результат (рекурсивно): {allEqualRec}");
        Console.WriteLine($"Максимальна глибина рекурсії: {maxDepth} (Теоретична log2(16) = 4)");

        Console.WriteLine("\n--- Завдання 3: Перебір із поверненням (підмножини із сумою 12) ---");
        int[] arr3 = GenerateArray(10, 1, 9, N);
        Console.WriteLine("Масив (10 чисел [1; 9]): [" + string.Join(", ", arr3) + "]");
        Console.WriteLine("Знайдені підмножини із сумою 12:");
        int solutionsCount = Task3_FindSubsets(arr3, 12);
        Console.WriteLine($"Всього знайдено підмножин: {solutionsCount}");

        Console.WriteLine("\n--- Завдання 4: Трасування (для масиву з 4 елементів) ---");
        int[] arr4 = { 5, 5, 5, 5 };
        Console.WriteLine("Масив: [" + string.Join(", ", arr4) + "]");
        Task4_TraceAllEqual(arr4, 0, arr4.Length - 1, 0);

        Console.WriteLine("\n--- Завдання 5: Ітеративна версія Завдання 2 ---");
        bool allEqualIter = Task5_AllEqualIterative(arr2);
        Console.WriteLine($"Результат (ітеративно): {allEqualIter}");
        Console.WriteLine($"Збігається з рекурсивною версією: {allEqualRec == allEqualIter}");
    }

    static int[] GenerateArray(int size, int min, int max, int seed)
    {
        Random rnd = new Random(seed);
        int[] arr = new int[size];
        for (int i = 0; i < size; i++)
        {
            arr[i] = rnd.Next(min, max + 1);
        }
        return arr;
    }

    static (int count, long size) Task1_DirectorySearch(DirectoryInfo dir)
    {
        int count = 0;
        long totalSize = 0;

        try
        {
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo f in files)
            {
                if (f.Extension.Equals(".cs", StringComparison.OrdinalIgnoreCase))
                {
                    count++;
                    totalSize += f.Length;
                }
            }

            DirectoryInfo[] subDirs = dir.GetDirectories();
            foreach (DirectoryInfo subDir in subDirs)
            {
                var (subCount, subSize) = Task1_DirectorySearch(subDir);
                count += subCount;
                totalSize += subSize;
            }
        }
        catch (UnauthorizedAccessException)
        {
        
        }
        catch (DirectoryNotFoundException)
        {
            
        }

        return (count, totalSize);
    }

    static bool Task2_AllEqual(int[] a, int lo, int hi, int currentDepth, ref int maxDepth)
    {
        if (currentDepth > maxDepth) maxDepth = currentDepth;

        if (lo >= hi) return true; // Базовий випадок

        int mid = lo + (hi - lo) / 2;
        bool leftEqual = Task2_AllEqual(a, lo, mid, currentDepth + 1, ref maxDepth);
        bool rightEqual = Task2_AllEqual(a, mid + 1, hi, currentDepth + 1, ref maxDepth);

        return leftEqual && rightEqual && (a[lo] == a[hi]);
    }

    static int Task3_FindSubsets(int[] arr, int targetSum)
    {
        List<int> current = new List<int>();
        int count = 0;
        Backtrack(arr, targetSum, 0, current, ref count);
        return count;
    }

    static void Backtrack(int[] arr, int target, int index, List<int> current, ref int count)
    {
        if (target == 0)
        {
            Console.WriteLine("[" + string.Join(", ", current) + "]");
            count++;
            return;
        }

        if (target < 0 || index >= arr.Length) return;

        current.Add(arr[index]);
        Backtrack(arr, target - arr[index], index + 1, current, ref count);
        current.RemoveAt(current.Count - 1);

        Backtrack(arr, target, index + 1, current, ref count);
    }

    static bool Task4_TraceAllEqual(int[] a, int lo, int hi, int depth)
    {
        string indent = new string(' ', depth * 2);
        Console.WriteLine($"{indent}enter AllEqual({lo}..{hi})");

        if (lo >= hi)
        {
            bool baseRes = true;
            Console.WriteLine($"{indent}exit  AllEqual({lo}..{hi}) -> {baseRes}");
            return baseRes;
        }

        int mid = lo + (hi - lo) / 2;
        bool left = Task4_TraceAllEqual(a, lo, mid, depth + 1);
        bool right = Task4_TraceAllEqual(a, mid + 1, hi, depth + 1);
        bool res = left && right && (a[lo] == a[hi]);

        Console.WriteLine($"{indent}exit  AllEqual({lo}..{hi}) -> {res}");
        return res;
    }

    static bool Task5_AllEqualIterative(int[] a)
    {
        if (a == null || a.Length <= 1) return true;

        int first = a[0];
        for (int i = 1; i < a.Length; i++)
        {
            if (a[i] != first) return false;
        }
        return true;
    }
}