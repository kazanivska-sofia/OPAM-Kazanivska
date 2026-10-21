using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OPAM_Lab6
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
            Console.WriteLine("N = 11, K = 22, a = 2, b = 3, c = 1\n");

            // --- Завдання 1 ---
            Console.WriteLine("=== Завдання 1: Частоти довжин слів ===");
            string text = "Алгоритм описує послідовність дій. Алгоритм можна записати словами, а можна записати кодом. Кодом алгоритм стає тоді, коли його виконує машина, а не людина. Одна задача — багато алгоритмів.";
            Task1_WordLengths(text);

            // --- Завдання 2 ---
            Console.WriteLine("\n=== Завдання 2: Групування студентів за кількістю літер у прізвищі ===");
            Task2_GroupStudents();

            // --- Завдання 3 ---
            Console.WriteLine("\n=== Завдання 3: Вилучення дублікатів ===");
            Task3_Deduplication();

            // --- Завдання 4 ---
            Console.WriteLine("\n=== Завдання 4: Елементи першого набору, відсутні в другому ===");
            Task4_SetDifference();

            // --- Завдання 5 ---
            Console.WriteLine("\n=== Завдання 5: Заміри швидкодії ===");
            Task5_Benchmarks();
        }

        // Завдання 1: a = 2 (довжини слів, поріг = 3)
        static void Task1_WordLengths(string text)
        {
            Dictionary<int, int> wordLengthFreq = new Dictionary<int, int>();
            char[] separators = new char[] { ' ', '.', ',', '—', '!', '?', ';' };
            string[] rawWords = text.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            foreach (string word in rawWords)
            {
                int length = word.Length;
                if (wordLengthFreq.ContainsKey(length))
                    wordLengthFreq[length]++;
                else
                    wordLengthFreq[length] = 1;
            }

            Console.WriteLine($"Загальна кількість різних довжин слів: {wordLengthFreq.Count}");
            Console.WriteLine("Довжини слів із частотою >= 3:");
            foreach (var pair in wordLengthFreq)
            {
                if (pair.Value >= 3)
                {
                    Console.WriteLine($"Довжина {pair.Key} літер: зустрічається {pair.Value} разів");
                }
            }
        }

        // Завдання 2: b = 3 (групування за кількістю літер у прізвищі)
        static void Task2_GroupStudents()
        {
            List<Student> students = new List<Student>
            {
                new Student("Коваленко", "КН-3/1", 88),
                new Student("Іщенко", "КН-3/1", 93),
                new Student("Савченко", "КН-3/2", 55),
                new Student("Кравець", "КН-3/1", 76),
                new Student("Мороз", "КН-3/2", 82),
                new Student("Бойко", "КН-3/2", 95),
                new Student("Мельник", "КН-3/2", 64),
                new Student("Бондар", "КН-3/1", 71),
                new Student("Ткачук", "КН-3/2", 90),
                new Student("Іванів", "КН-3/1", 48),
                new Student("Шевчук", "КН-3/1", 67),
                new Student("Кушнір", "КН-3/2", 73)
            };

            Dictionary<int, List<Student>> groups = new Dictionary<int, List<Student>>();

            foreach (var student in students)
            {
                int nameLength = student.Name.Length;
                if (!groups.ContainsKey(nameLength))
                {
                    groups[nameLength] = new List<Student>();
                }
                groups[nameLength].Add(student);
            }

            foreach (var pair in groups)
            {
                double sum = 0;
                foreach (var st in pair.Value)
                {
                    sum += st.Score;
                }
                double avg = sum / pair.Value.Count;

                Console.WriteLine($"Група (Прізвище з {pair.Key} літер): кількість = {pair.Value.Count}, середній бал = {avg:F2}");
            }
        }

        // Завдання 3: Вилучення дублікатів (25 чисел, діапазон [1; 8], Random(11))
        static void Task3_Deduplication()
        {
            Random rnd = new Random(11);
            List<int> initial = new List<int>();
            for (int i = 0; i < 25; i++)
            {
                initial.Add(rnd.Next(1, 9));
            }

            // Спосіб 1: Без збереження порядку
            HashSet<int> method1 = new HashSet<int>(initial);

            // Спосіб 2: Зі збереженням порядку
            HashSet<int> seen = new HashSet<int>();
            List<int> method2 = new List<int>();
            foreach (int item in initial)
            {
                if (seen.Add(item))
                {
                    method2.Add(item);
                }
            }

            Console.WriteLine("Початковий список: " + string.Join(", ", initial));
            Console.WriteLine("Спосіб 1 (HashSet): " + string.Join(", ", method1));
            Console.WriteLine("Спосіб 2 (збереження порядку): " + string.Join(", ", method2));
        }

        // Завдання 4: c = 1 (елементи першого набору, яких немає у другому)
        static void Task4_SetDifference()
        {
            Random rnd1 = new Random(11);
            Random rnd2 = new Random(111);

            List<int> list1 = new List<int>();
            List<int> list2 = new List<int>();

            for (int i = 0; i < 30; i++)
            {
                list1.Add(rnd1.Next(1, 43));
                list2.Add(rnd2.Next(1, 43));
            }

            // Спосіб 1: Через HashSet
            HashSet<int> set2 = new HashSet<int>(list2);
            HashSet<int> resultSet = new HashSet<int>();
            foreach (int x in list1)
            {
                if (!set2.Contains(x))
                {
                    resultSet.Add(x);
                }
            }

            // Спосіб 2: Вкладені цикли по списках
            List<int> resultList = new List<int>();
            foreach (int x in list1)
            {
                bool found = false;
                foreach (int y in list2)
                {
                    if (x == y)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    bool alreadyInResult = false;
                    foreach (int r in resultList)
                    {
                        if (r == x)
                        {
                            alreadyInResult = true;
                            break;
                        }
                    }
                    if (!alreadyInResult)
                    {
                        resultList.Add(x);
                    }
                }
            }

            Console.WriteLine("Результат (HashSet): " + string.Join(", ", resultSet));
            Console.WriteLine("Результат (Списки):  " + string.Join(", ", resultList));
        }

        // Завдання 5: Вимірювання швидкодії (Stopwatch)
        static void Task5_Benchmarks()
        {
            Stopwatch sw = new Stopwatch();

            // 10 000 перевірок наявності у наборі зі 100 000 елементів
            List<int> bigList = new List<int>();
            for (int i = 0; i < 100000; i++) bigList.Add(i);
            HashSet<int> bigSet = new HashSet<int>(bigList);

            sw.Start();
            for (int i = 0; i < 10000; i++) bigList.Contains(-1);
            sw.Stop();
            long listTime = sw.ElapsedMilliseconds;

            sw.Restart();
            for (int i = 0; i < 10000; i++) bigSet.Contains(-1);
            sw.Stop();
            long setTime = sw.ElapsedMilliseconds;

            Console.WriteLine($"10 000 пошуків: Список = {listTime} мс, HashSet = {setTime} мс");
        }
    }
}