using System;
using System.Collections.Generic;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        int N = 11;
        int K = 22;

        Console.WriteLine($"=== ЛАБОРАТОРНА РОБОТА №4 (N={N}, K={K}) ===\n");

        List<int> mainList = GenerateList(20, -9, 9, N);
        Console.WriteLine("Основний список (20 чисел [-9; 9]):");
        PrintList(mainList);

        Console.WriteLine("\n--- Завдання 1: Побудова нового списку (додатні й непарні) ---");
        List<int> task1Result = Task1_FilterPositiveOdd(mainList);
        Console.Write("Результат: ");
        PrintList(task1Result);
        Console.WriteLine($"Кількість елементів (Count): {task1Result.Count}");

        Console.WriteLine("\n--- Завдання 2: Вилучення елементів (кратні 3) ---");
        List<int> task2Wrong = Task2_WrongRemove(mainList);
        List<int> task2Backwards = Task2_RemoveBackwards(mainList);
        List<int> task2NewList = Task2_RemoveNewList(mainList);

        Console.Write("1. Хибний варіант (з початку): ");
        PrintList(task2Wrong);
        Console.Write("2. Правильний (з кінця):     ");
        PrintList(task2Backwards);
        Console.Write("3. Правильний (новий список): ");
        PrintList(task2NewList);

        bool task2Equals = AreListsEqual(task2Backwards, task2NewList);
        Console.WriteLine($"Результати правильних варіантів збігаються: {task2Equals}");

        Console.WriteLine("\n--- Завдання 3: Злиття двох впорядкованих списків ---");
        int len1 = 10 + (K % 5); // 10 + 2 = 12
        int len2 = 6 + (K % 4);  // 6 + 2 = 8
        List<int> sorted1 = GenerateSortedList(len1, N);
        List<int> sorted2 = GenerateSortedList(len2, N + 100);

        Console.Write($"Список 1 (довжина {len1}): ");
        PrintList(sorted1);
        Console.Write($"Список 2 (довжина {len2}): ");
        PrintList(sorted2);

        List<int> merged = Task3_MergeSorted(sorted1, sorted2);
        Console.Write("Злитий список: ");
        PrintList(merged);

        bool isSorted = CheckIsSorted(merged);
        Console.WriteLine($"Перевірка впорядкованості: {isSorted}");

        Console.WriteLine("\n--- Завдання 4: Розділення списку (парні та непарні індекси) ---");
        var (evenIndexList, oddIndexList) = Task4_SplitByIndices(mainList);
        Console.Write("Елементи на парних індексах:  ");
        PrintList(evenIndexList);
        Console.Write("Елементи на непарних індексах: ");
        PrintList(oddIndexList);

        bool countSumEquals = (evenIndexList.Count + oddIndexList.Count) == mainList.Count;
        Console.WriteLine($"Сума Count списків дорівнює вихідному: {countSumEquals}");

        Console.WriteLine("\n--- Завдання 5: Заміри часу ---");
        RunTimeBenchmark();
    }

    static List<int> GenerateList(int count, int min, int max, int seed)
    {
        Random rnd = new Random(seed);
        List<int> list = new List<int>(count);
        for (int i = 0; i < count; i++)
        {
            list.Add(rnd.Next(min, max + 1));
        }
        return list;
    }

    static List<int> GenerateSortedList(int count, int seed)
    {
        Random rnd = new Random(seed);
        List<int> list = new List<int>(count);
        int current = rnd.Next(1, 10);
        for (int i = 0; i < count; i++)
        {
            list.Add(current);
            current += rnd.Next(1, 10);
        }
        return list;
    }

    static void PrintList(List<int> list)
    {
        Console.WriteLine("[" + string.Join(", ", list) + "]");
    }

    static bool AreListsEqual(List<int> l1, List<int> l2)
    {
        if (l1.Count != l2.Count) return false;
        for (int i = 0; i < l1.Count; i++)
        {
            if (l1[i] != l2[i]) return false;
        }
        return true;
    }

    static bool CheckIsSorted(List<int> list)
    {
        for (int i = 1; i < list.Count; i++)
        {
            if (list[i] < list[i - 1]) return false;
        }
        return true;
    }

    static List<int> Task1_FilterPositiveOdd(List<int> input)
    {
        List<int> result = new List<int>();
        for (int i = 0; i < input.Count; i++)
        {
            int val = input[i];
            if (val > 0 && val % 2 != 0)
            {
                result.Add(val);
            }
        }
        return result;
    }

    static List<int> Task2_WrongRemove(List<int> input)
    {
        List<int> list = new List<int>(input);
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] % 3 == 0)
            {
                list.RemoveAt(i);
            }
        }
        return list;
    }

    static List<int> Task2_RemoveBackwards(List<int> input)
    {
        List<int> list = new List<int>(input);
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i] % 3 == 0)
            {
                list.RemoveAt(i);
            }
        }
        return list;
    }

    static List<int> Task2_RemoveNewList(List<int> input)
    {
        List<int> result = new List<int>();
        for (int i = 0; i < input.Count; i++)
        {
            if (input[i] % 3 != 0)
            {
                result.Add(input[i]);
            }
        }
        return result;
    }

    static List<int> Task3_MergeSorted(List<int> l1, List<int> l2)
    {
        List<int> merged = new List<int>(l1.Count + l2.Count);
        int i = 0, j = 0;

        while (i < l1.Count && j < l2.Count)
        {
            if (l1[i] <= l2[j])
            {
                merged.Add(l1[i]);
                i++;
            }
            else
            {
                merged.Add(l2[j]);
                j++;
            }
        }

        while (i < l1.Count)
        {
            merged.Add(l1[i]);
            i++;
        }

        while (j < l2.Count)
        {
            merged.Add(l2[j]);
            j++;
        }

        return merged;
    }

    static (List<int> even, List<int> odd) Task4_SplitByIndices(List<int> input)
    {
        List<int> evenIndices = new List<int>();
        List<int> oddIndices = new List<int>();

        for (int i = 0; i < input.Count; i++)
        {
            if (i % 2 == 0)
            {
                evenIndices.Add(input[i]);
            }
            else
            {
                oddIndices.Add(input[i]);
            }
        }

        return (evenIndices, oddIndices);
    }

    static void RunTimeBenchmark()
    {
        int count = 100000;

        List<int> warmUp = new List<int>();
        for (int i = 0; i < 1000; i++) warmUp.Add(i);

        Stopwatch sw1 = Stopwatch.StartNew();
        List<int> list1 = new List<int>();
        for (int i = 0; i < count; i++) list1.Add(i);
        sw1.Stop();

        Stopwatch sw2 = Stopwatch.StartNew();
        List<int> list2 = new List<int>(count);
        for (int i = 0; i < count; i++) list2.Add(i);
        sw2.Stop();

        Stopwatch sw3 = Stopwatch.StartNew();
        List<int> list3 = new List<int>();
        for (int i = 0; i < count; i++) list3.Insert(0, i);
        sw3.Stop();

        Stopwatch sw4 = Stopwatch.StartNew();
        for (int i = 0; i < 1000; i++)
        {
            bool found = list1.Contains(-999999);
        }
        sw4.Stop();

        Console.WriteLine($"100 000 x Add(x) без місткості:      {sw1.ElapsedMilliseconds} ms");
        Console.WriteLine($"100 000 x Add(x) з new List(100000): {sw2.ElapsedMilliseconds} ms");
        Console.WriteLine($"100 000 x Insert(0, x):              {sw3.ElapsedMilliseconds} ms");
        Console.WriteLine($"1 000 x Contains(-999999):           {sw4.ElapsedMilliseconds} ms");
    }
}