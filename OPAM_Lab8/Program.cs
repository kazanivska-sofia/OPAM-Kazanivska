using System;
using System.Diagnostics;

namespace OPAM_Lab8
{
    public class Student
    {
        public string Name { get; set; }
        public int Score { get; set; }

        public Student(string name, int score)
        {
            Name = name;
            Score = score;
        }
    }

    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== ЛАБОРАТОРНА РОБОТА №8 (N=11, K=22, a=2, b=3, c=1) ===\n");

            int n = 100000;
            Student[] students = new Student[n];
            Random rnd = new Random(11);

            for (int i = 0; i < n; i++)
            {
                students[i] = new Student($"Student_{i}", rnd.Next(0, 101));
            }

            Func<Student, Student, int> cmp = (s1, s2) => s1.Score.CompareTo(s2.Score);

            // Сортуємо масив для бінарного пошуку
            Array.Sort(students, (s1, s2) => cmp(s1, s2));

            Student targetX = new Student("TargetX", 72);
            Student targetFirst = students[0];
            Student targetMissing = new Student("Missing", 101);

            // --- ЗАВДАННЯ 1 ---
            Console.WriteLine("--- Завдання 1: Лінійний пошук ---");
            Search.LinearSearch(students, targetX, cmp, out int c1);
            Console.WriteLine($"Запит X (72 бали): порівнянь = {c1}");
            Search.LinearSearch(students, targetFirst, cmp, out int c2);
            Console.WriteLine($"Перший елемент: порівнянь = {c2}");
            Search.LinearSearch(students, targetMissing, cmp, out int c3);
            Console.WriteLine($"Відсутнє (101 бал): порівнянь = {c3}\n");

            // --- ЗАВДАННЯ 2 ---
            Console.WriteLine("--- Завдання 2: Бінарний пошук та Поломки ---");
            Search.BinarySearch(students, targetX, cmp, out int bc1);
            Console.WriteLine($"Правильний Бінарний (X): порівнянь = {bc1}");

            Broken.Broken1(students, targetX, cmp, out int br1);
            Console.WriteLine($"Поломка 1 (lo < hi): порівнянь = {br1}");
            Console.WriteLine("Поломка 2 (lo = mid): викликає нескінченне зациклення!");
            Console.WriteLine("Поломка 3 (mid = (lo+hi)/2): працює на 100 000, але ламається при сумі > int.MaxValue");

            Student[] unsorted = new Student[100];
            for (int i = 0; i < 100; i++) unsorted[i] = new Student($"S_{i}", rnd.Next(0, 101));
            int failedCount = 0;
            for (int i = 0; i < 100; i++)
            {
                int idx = Search.BinarySearch(unsorted, unsorted[i], cmp, out _);
                if (idx == -1 || unsorted[idx].Score != unsorted[i].Score) failedCount++;
            }
            Console.WriteLine($"Поломка 4 (невідсортований масив): {failedCount} із 100 запитів повернули помилку!\n");

            // --- ЗАВДАННЯ 3 ---
            Console.WriteLine("--- Завдання 3: Пошук межі (b = 3: Кількість у [0; 72]) ---");
            int firstIdx = Search.LowerBound(students, s => s.Score >= 0, out int lb1);
            int lastIdx = Search.LowerBound(students, s => s.Score > 72, out int lb2);
            int countInRange = lastIdx - firstIdx;
            Console.WriteLine($"Кількість студентів з балами від 0 до 72: {countInRange} (Пошук межі зайняв {lb1 + lb2} порівнянь)\n");

            // --- ЗАВДАННЯ 4 ---
            Console.WriteLine("--- Завдання 4: Варіант c = 1 (Без перевірки рівності в циклі) ---");
            Search.BinarySearchNoEqCheck(students, targetX, cmp, out int cNoEq);
            Console.WriteLine($"Бінарний без рівності у циклі: порівнянь = {cNoEq} (на 1 порівняння менше на кожному кроці!)\n");

            // --- ЗАВДАННЯ 5 ---
            Console.WriteLine("--- Завдання 5: Заміри точки окупності ---");
            RunBenchmarks(n, cmp);
        }

        static void RunBenchmarks(int n, Func<Student, Student, int> cmp)
        {
            Student[] data = new Student[n];
            Random rnd = new Random(11);
            for (int i = 0; i < n; i++) data[i] = new Student($"S_{i}", rnd.Next(0, 101));

            Stopwatch sw = new Stopwatch();

            // Точка окупності
            int[] queries = { 1, 5, 10, 20, 30, 50, 100 };
            Console.WriteLine("q\tЛінійно (мс)\tСортування + Бінарно (мс)\tХто швидший");

            foreach (int q in queries)
            {
                // Лінійний пошук q разів
                sw.Restart();
                for (int i = 0; i < q; i++)
                {
                    Student target = new Student("T", rnd.Next(0, 101));
                    Search.LinearSearch(data, target, cmp, out _);
                }
                sw.Stop();
                long tLinear = sw.ElapsedMilliseconds;

                // Сортування + Бінарний пошук q разів
                sw.Restart();
                Student[] copy = (Student[])data.Clone();
                Array.Sort(copy, (s1, s2) => cmp(s1, s2));
                for (int i = 0; i < q; i++)
                {
                    Student target = new Student("T", rnd.Next(0, 101));
                    Search.BinarySearch(copy, target, cmp, out _);
                }
                sw.Stop();
                long tBinary = sw.ElapsedMilliseconds;

                Console.WriteLine($"{q}\t{tLinear}\t\t{tBinary}\t\t\t{(tLinear < tBinary ? "Лінійний" : "Бінарний")}");
            }
        }
    }
}