using System;
using System.Collections.Generic;

namespace Lab05
{
    // Завдання 4: Власний стек поверх масиву
    public class MyStack<T>
    {
        private T[] _items;
        private int _count;

        public int Count => _count;
        public int Capacity => _items.Length;

        public MyStack(int initialCapacity = 4)
        {
            _items = new T[initialCapacity];
            _count = 0;
        }

        public void Push(T item)
        {
            if (_count == _items.Length)
            {
                Array.Resize(ref _items, _items.Length * 2);
            }
            _items[_count++] = item;
        }

        public T Pop()
        {
            if (_count == 0)
                throw new InvalidOperationException("Стек порожній!");
            
            T item = _items[--_count];
            _items[_count] = default!;
            return item;
        }

        public T Peek()
        {
            if (_count == 0)
                throw new InvalidOperationException("Стек порожній!");
            return _items[_count - 1];
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== ЗАВДАННЯ 1: Збалансованість дужок ===");
            TestBracketCheck();

            Console.WriteLine("\n=== ЗАВДАННЯ 2: Зворотний польський запис ===");
            TestRPN();

            Console.WriteLine("\n=== ЗАВДАННЯ 3: Модель черги обслуговування ===");
            SimulateQueue();

            Console.WriteLine("\n=== ЗАВДАННЯ 4: Перевірка власного стеку ===");
            TestMyStack();
        }

        // --- ЗАВДАННЯ 1 ---
        public static (bool IsBalanced, int ErrorIndex) CheckBrackets(string input, bool useCustomStack = false)
        {
            bool inQuotes = false;
            Stack<char> stdStack = new Stack<char>();
            MyStack<char> customStack = new MyStack<char>(4);
            Stack<(char Symbol, int Index)> indexStack = new Stack<(char, int)>();

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];

                if (c == '"')
                {
                    inQuotes = !inQuotes;
                    continue;
                }

                if (inQuotes) continue;

                if (c == '(' || c == '[' || c == '{')
                {
                    if (useCustomStack) customStack.Push(c);
                    else stdStack.Push(c);
                    indexStack.Push((c, i));
                }
                else if (c == ')' || c == ']' || c == '}')
                {
                    int currentCount = useCustomStack ? customStack.Count : stdStack.Count;
                    if (currentCount == 0) return (false, i);

                    char open = useCustomStack ? customStack.Pop() : stdStack.Pop();
                    indexStack.Pop();

                    if (!IsMatchingPair(open, c)) return (false, i);
                }
            }

            if (inQuotes) return (false, input.Length - 1);

            int finalCount = useCustomStack ? customStack.Count : stdStack.Count;
            if (finalCount > 0)
            {
                var unclosed = indexStack.Pop();
                return (false, unclosed.Index);
            }

            return (true, -1);
        }

        private static bool IsMatchingPair(char open, char close) =>
            (open == '(' && close == ')') ||
            (open == '[' && close == ']') ||
            (open == '{' && close == '}');

        private static void TestBracketCheck()
        {
            string[] tests = { "\"(a[b]{c})\"", "\"([)]\"", "\")(\"", "\"((\"", "\"\"" };
            foreach (var t in tests)
            {
                var res = CheckBrackets(t);
                Console.WriteLine($"Рядок: {t,-12} -> {(res.IsBalanced ? "збалансований" : "незбалансований")}, Індекс помилки: {res.ErrorIndex}");
            }

            string testK = new string('(', 22) + new string(')', 21);
            var resK = CheckBrackets(testK);
            Console.WriteLine($"Рядок з K=22 (22'(' i 21')') -> {(resK.IsBalanced ? "збалансований" : "незбалансований")}, Індекс помилки: {resK.ErrorIndex}");
        }

        // --- ЗАВДАННЯ 2 ---
        public static (bool Success, double Result, string ErrorMessage) EvaluateRPN(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
                return (true, 0, "Порожній вираз");

            string[] tokens = expression.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            Stack<double> stack = new Stack<double>();

            foreach (var token in tokens)
            {
                if (double.TryParse(token, out double number))
                {
                    stack.Push(number);
                }
                else if (token == "+" || token == "-" || token == "*" || token == "/" || token == "min" || token == "max")
                {
                    if (stack.Count < 2)
                        return (false, 0, "Забракло чисел для операції");

                    double b = stack.Pop();
                    double a = stack.Pop();

                    switch (token)
                    {
                        case "+": stack.Push(a + b); break;
                        case "-": stack.Push(a - b); break;
                        case "*": stack.Push(a * b); break;
                        case "/":
                            if (Math.Abs(b) < 1e-9) return (false, 0, "Ділення на нуль");
                            stack.Push(a / b);
                            break;
                        case "min": stack.Push(Math.Min(a, b)); break;
                        case "max": stack.Push(Math.Max(a, b)); break;
                    }
                }
                else
                {
                    return (false, 0, $"Невідомий токен: '{token}'");
                }
            }

            if (stack.Count != 1)
                return (false, 0, "Забагато чисел у виразі");

            return (true, stack.Pop(), "OK");
        }

        private static void TestRPN()
        {
            string[] mandatoryTests = { "3 4 + 2 *", "5 3 -", "8 2 /", "2 3 4 * +", "5 0 /" };
            foreach (var t in mandatoryTests)
            {
                var r = EvaluateRPN(t);
                if (r.Success) Console.WriteLine($"\"{t}\" -> {r.Result}");
                else Console.WriteLine($"\"{t}\" -> Помилка: {r.ErrorMessage}");
            }

            string myExpr = "10 30 min 12 +";
            var myRes = EvaluateRPN(myExpr);
            Console.WriteLine($"Власний вираз \"{myExpr}\" -> Результат: {myRes.Result}");
        }

        // --- ЗАВДАННЯ 3 ---
        private static void SimulateQueue()
        {
            Random rand = new Random(11);
            int totalRequests = 10;
            Queue<(int Id, int Weight, int ArrivalTime)> queue = new Queue<(int, int, int)>();

            int currentTime = 0;
            int totalIdleTime = 0;
            int maxQueueLength = 0;

            int arrivalTime = 0;
            for (int i = 1; i <= totalRequests; i++)
            {
                arrivalTime += rand.Next(0, 4);
                int weight = rand.Next(1, 10);
                queue.Enqueue((i, weight, arrivalTime));
            }

            maxQueueLength = queue.Count;

            while (queue.Count > 0)
            {
                if (queue.Count > maxQueueLength) maxQueueLength = queue.Count;

                if (queue.Count > 0)
                {
                    var req = queue.Dequeue();

                    if (currentTime < req.ArrivalTime)
                    {
                        totalIdleTime += (req.ArrivalTime - currentTime);
                        currentTime = req.ArrivalTime;
                    }

                    int startTime = currentTime;
                    int endTime = startTime + req.Weight;
                    currentTime = endTime;

                    Console.WriteLine($"Заявка #{req.Id}: Вага = {req.Weight} хв, Надійшла = {req.ArrivalTime} хв, Початок = {startTime} хв, Завершення = {endTime} хв");
                }
            }

            Console.WriteLine($"Максимальна довжина черги: {maxQueueLength}");
            Console.WriteLine($"Загальний час простою обслуговувача: {totalIdleTime} хв");
        }

        // --- ЗАВДАННЯ 4 ---
        private static void TestMyStack()
        {
            MyStack<int> stack = new MyStack<int>(initialCapacity: 4);
            Console.WriteLine($"Початкова місткість: {stack.Capacity}");

            for (int i = 1; i <= 5; i++)
            {
                stack.Push(i * 10);
                Console.WriteLine($"Push({i * 10}): Count = {stack.Count}, Capacity = {stack.Capacity}");
            }

            string testStr = "\"(a[b]{c})\"";
            var res = CheckBrackets(testStr, useCustomStack: true);
            Console.WriteLine($"Перевірка Завдання 1 з MyStack: \"{testStr}\" -> {(res.IsBalanced ? "збалансований" : "незбалансований")}");
        }
    }
}