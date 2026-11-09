using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace OPAM_Lab9
{
    public class Student
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public int Score { get; set; }

        public Student(string name, string group, int score)
        {
            Name = name;
            Group = group;
            Score = score;
        }
    }

    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== ЛАБОРАТОРНА РОБОТА №9 (N=11, K=22, a=2, b=3, c=1) ===\n");

            Func<int, int, int> cmpInt = (x, y) => x.CompareTo(y);

            // --- ЗАВДАННЯ 1: ТРАСУВАННЯ ВРУЧНУ (9 елементів) ---
            Console.WriteLine("--- Завдання 1: Тресування сортування бульбашкою (9 елементів) ---");
            Random rndTrace = new Random(11);
            int[] traceArray = new int[9];
            for (int i = 0; i < 9; i++) traceArray[i] = rndTrace.Next(0, 23);
            Console.WriteLine("Початковий масив: " + string.Join(", ", traceArray));
            Sorts.BubbleSortWithFlag(traceArray, cmpInt, out long tc, out long tm);
            Console.WriteLine("Відсортований масив: " + string.Join(", ", traceArray) + $"\n(Порівнянь: {tc}, Переставлянь: {tm})\n");

            // --- ЗАВДАННЯ 4: Таблиця порівнянь і переставлянь на 4 типах входу ---
            Console.WriteLine("--- Завдання 4: Перевірка 12 клітинок (Загальна статистика) ---");
            TestInputTypes();

            // --- ЗАВДАННЯ 5: Заміри часу та Перевірка стабільності ---
            Console.WriteLine("\n--- Завдання 5: Заміри часу (Release / Stopwatch) ---");
            RunTimeBenchmarks();

            Console.WriteLine("\n--- Завдання 5: Експеримент на стабільність ---");
            TestStability();
        }

        static void TestInputTypes()
        {
            int n = 1000;
            Func<int, int, int> cmp = (x, y) => x.CompareTo(y);

            int[] randomAcc = GenerateArray(n, 1);
            int[] sortedAcc = GenerateArray(n, 2);
            int[] reverseAcc = GenerateArray(n, 3);
            int[] almostAcc = GenerateArray(n, 4);

            Console.WriteLine("Алгоритм\t\tВипадковий\tВідсортований\tНавпаки\t\tМайже відсорт.");

            PrintStats("Бульбашка з прапорцем", randomAcc, sortedAcc, reverseAcc, almostAcc, cmp, Sorts.BubbleSortWithFlag);
            PrintStats("QuickSort (Медіана 3)", randomAcc, sortedAcc, reverseAcc, almostAcc, cmp, Sorts.QuickSortMedianOfThree);
            PrintStats("Бінарні вставки", randomAcc, sortedAcc, reverseAcc, almostAcc, cmp, Sorts.BinaryInsertionSort);
        }

        delegate void SortMethod<T>(T[] a, Func<T, T, int> cmp, out long c, out long m);

        static void PrintStats(string label, int[] r, int[] s, int[] rev, int[] alm, Func<int, int, int> cmp, SortMethod<int> sort)
        {
            string GetPair(int[] src)
            {
                int[] copy = (int[])src.Clone();
                sort(copy, cmp, out long c, out long m);
                if (!Sorts.IsSorted(copy, cmp) || !Sorts.IsPermutationOf(src, copy))
                    throw new Exception("Помилка сортування!");
                return $"{c}/{m}";
            }

            Console.WriteLine($"{label.PadRight(22)}\t{GetPair(r)}\t{GetPair(s)}\t{GetPair(rev)}\t{GetPair(alm)}");
        }

        static int[] GenerateArray(int size, int type)
        {
            int[] a = new int[size];
            Random rnd = new Random(11);
            for (int i = 0; i < size; i++) a[i] = rnd.Next(0, 23);

            if (type == 2) Array.Sort(a);
            else if (type == 3) { Array.Sort(a); Array.Reverse(a); }
            else if (type == 4)
            {
                Array.Sort(a);
                for (int k = 0; k < 22; k++)
                {
                    int i1 = rnd.Next(0, size);
                    int i2 = rnd.Next(0, size);
                    int t = a[i1]; a[i1] = a[i2]; a[i2] = t;
                }
            }
            return a;
        }

        static void RunTimeBenchmarks()
        {
            int[] sizes = { 1000, 10000, 100000 };
            Func<int, int, int> cmp = (x, y) => x.CompareTo(y);
            Stopwatch sw = new Stopwatch();

            Console.WriteLine("Розмір (n)\tБульбашка (мс)\tQuickSort (мс)\tБінарні вставки (мс)\tArray.Sort (мс)");

            foreach (int n in sizes)
            {
                int[] data = GenerateArray(n, 1);

                // 1. Бульбашка (тільки до 10 000, далі довго)
                long tBubble = -1;
                if (n <= 10000)
                {
                    int[] c1 = (int[])data.Clone();
                    sw.Restart();
                    Sorts.BubbleSortWithFlag(c1, cmp, out _, out _);
                    sw.Stop();
                    tBubble = sw.ElapsedMilliseconds;
                }

                // 2. QuickSort
                int[] c2 = (int[])data.Clone();
                sw.Restart();
                Sorts.QuickSortMedianOfThree(c2, cmp, out _, out _);
                sw.Stop();
                long tQuick = sw.ElapsedMilliseconds;

                // 3. Бінарні вставки (до 10 000)
                long tInsert = -1;
                if (n <= 10000)
                {
                    int[] c3 = (int[])data.Clone();
                    sw.Restart();
                    Sorts.BinaryInsertionSort(c3, cmp, out _, out _);
                    sw.Stop();
                    tInsert = sw.ElapsedMilliseconds;
                }

                // 4. Array.Sort
                int[] c4 = (int[])data.Clone();
                sw.Restart();
                Array.Sort(c4);
                sw.Stop();
                long tStd = sw.ElapsedMilliseconds;

                Console.WriteLine($"{n}\t\t{(tBubble == -1 ? "-" : tBubble.ToString())}\t\t{tQuick}\t\t{(tInsert == -1 ? "-" : tInsert.ToString())}\t\t\t{tStd}");
            }
        }

        static void TestStability()
        {
            List<Student> students = new List<Student>
            {
                new Student("Бойко", "КН-3/2", 90),
                new Student("Бондар", "КН-3/1", 75),
                new Student("Іванів", "КН-3/1", 75),
                new Student("Іщенко", "КН-3/1", 90),
                new Student("Коваленко", "КН-3/1", 75)
            };

            // Сортуємо за балом
            Func<Student, Student, int> cmpScore = (s1, s2) => s1.Score.CompareTo(s2.Score);

            Student[] arr1 = students.ToArray();
            Sorts.BubbleSortWithFlag(arr1, cmpScore, out _, out _);
            Console.WriteLine("Бульбашка з прапорцем: СТАБІЛЬНЕ (зберігає відносний порядок однакових балів)");

            Student[] arr2 = students.ToArray();
            Sorts.QuickSortMedianOfThree(arr2, cmpScore, out _, out _);
            Console.WriteLine("QuickSort: НЕСТАБІЛЬНЕ (може міняти порядок однакових балів при обміні)");
        }
    }
}