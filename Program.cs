using System;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        int N = 11;
        int K = 22;

        Console.WriteLine($"=== ЛАБОРАТОРНА РОБОТА №2 (N={N}, K={K}) ===");

        // Заповнення масиву за варіантом b = 3: 18 елементів, [-3; 3], Random(11)
        int[] originalArray = GenerateArray(18, -3, 3, N);
        
        Console.WriteLine("\n--- Вхідний масив (18 елементів) ---");
        PrintArray(originalArray);

        // --- Завдання 1 ---
        Console.WriteLine("\n--- Завдання 1: Агрегація за один прохід ---");
        Task1(originalArray);

        // --- Завдання 2 ---
        Console.WriteLine("\n--- Завдання 2: Фільтрація (елементи < 2) ---");
        int[] filtered = Task2(originalArray, 2);
        Console.Write("Відфільтрований масив: ");
        PrintArray(filtered);

        // --- Завдання 3 ---
        Console.WriteLine("\n--- Завдання 3: Найдовша серія однакових елементів ---");
        Task3(originalArray);

        // --- Завдання 4 ---
        Console.WriteLine("\n--- Завдання 4: Перестановка (циклічний зсув вліво на 1) ---");
        int[] arrayToShift = (int[])originalArray.Clone(); // клонуємо для збереження початкового
        Task4(arrayToShift);
        Console.Write("Масив після зсуву: ");
        PrintArray(arrayToShift);

        // --- Завдання 5 ---
        Console.WriteLine("\n--- Завдання 5: Матриця 6x3 ---");
        int[,] matrix = GenerateMatrix(6, 3, -3, 3, N);
        Task5(matrix);
    }

    // Генерація масиву
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

    // Виведення масиву
    static void PrintArray(int[] arr)
    {
        if (arr == null || arr.Length == 0)
        {
            Console.WriteLine("[Порожній масив]");
            return;
        }
        Console.WriteLine("[" + string.Join(", ", arr) + "]");
    }

    // Завдання 1: Агрегація
    static void Task1(int[] arr)
    {
        if (arr == null || arr.Length == 0)
        {
            Console.WriteLine("Результат не визначений: масив порожній.");
            return;
        }

        int min = arr[0], max = arr[0];
        int minIdx = 0, maxIdx = 0;
        long sum = 0;
        int zeroCount = 0;

        for (int i = 0; i < arr.Length; i++)
        {
            int val = arr[i];
            sum += val;

            if (val == 0) zeroCount++;

            if (val < min)
            {
                min = val;
                minIdx = i;
            }

            if (val > max)
            {
                max = val;
                maxIdx = i;
            }
        }

        double avg = (double)sum / arr.Length;

        Console.WriteLine($"Сума: {sum}");
        Console.WriteLine($"Середнє арифметичне: {avg:F2}");
        Console.WriteLine($"Мінімум: {min} (індекс: {minIdx})");
        Console.WriteLine($"Максимум: {max} (індекс першого входження: {maxIdx})");
        Console.WriteLine($"Кількість нулів: {zeroCount}");
    }

    // Завдання 2: Фільтрація (2 проходи)
    static int[] Task2(int[] arr, int threshold)
    {
        if (arr == null || arr.Length == 0) return new int[0];

        int count = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] < threshold) count++;
        }

        int[] result = new int[count];
        int idx = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] < threshold)
            {
                result[idx++] = arr[i];
            }
        }

        return result;
    }

    // Завдання 3: Найдовша серія
    static void Task3(int[] arr)
    {
        if (arr == null || arr.Length == 0)
        {
            Console.WriteLine("Масив порожній.");
            return;
        }

        int bestValue = arr[0];
        int bestLen = 1;
        int bestStart = 0;

        int currentVal = arr[0];
        int currentLen = 1;
        int currentStart = 0;

        for (int i = 1; i < arr.Length; i++)
        {
            if (arr[i] == currentVal)
            {
                currentLen++;
            }
            else
            {
                if (currentLen > bestLen)
                {
                    bestLen = currentLen;
                    bestValue = currentVal;
                    bestStart = currentStart;
                }
                currentVal = arr[i];
                currentLen = 1;
                currentStart = i;
            }
        }

        // Перевірка серії, що закінчується на останньому елементі
        if (currentLen > bestLen)
        {
            bestLen = currentLen;
            bestValue = currentVal;
            bestStart = currentStart;
        }

        Console.WriteLine($"Значення серії: {bestValue}");
        Console.WriteLine($"Довжина серії: {bestLen}");
        Console.WriteLine($"Початковий індекс: {bestStart}");
    }

    // Завдання 4: Зсув вліво in-place (без другого масиву)
    static void Task4(int[] arr)
    {
        if (arr == null || arr.Length <= 1) return;

        int first = arr[0];
        for (int i = 0; i < arr.Length - 1; i++)
        {
            arr[i] = arr[i + 1];
        }
        arr[arr.Length - 1] = first;
    }

    // Завдання 5: Матриця
    static int[,] GenerateMatrix(int rows, int cols, int min, int max, int seed)
    {
        Random rnd = new Random(seed);
        int[,] mat = new int[rows, cols];
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                mat[r, c] = rnd.Next(min, max + 1);
            }
        }
        return mat;
    }

    static void Task5(int[,] mat)
    {
        int rows = mat.GetLength(0);
        int cols = mat.GetLength(1);

        Console.WriteLine("Матриця:");
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                Console.Write($"{mat[r, c],4}");
            }
            Console.WriteLine();
        }

        int maxSum = int.MinValue;
        int maxRowIdx = 0;

        Console.WriteLine("\nСуми рядків:");
        for (int r = 0; r < rows; r++)
        {
            int rowSum = 0;
            for (int c = 0; c < cols; c++)
            {
                rowSum += mat[r, c];
            }
            Console.WriteLine($"Рядок {r}: сума = {rowSum}");

            if (rowSum > maxSum)
            {
                maxSum = rowSum;
                maxRowIdx = r;
            }
        }

        Console.WriteLine($"\nНомер рядка з найбільшою сумою: {maxRowIdx} (сума = {maxSum})");

        Console.WriteLine("\nМаксимуми стовпчиків:");
        for (int c = 0; c < cols; c++)
        {
            int colMax = mat[0, c];
            for (int r = 1; r < rows; r++)
            {
                if (mat[r, c] > colMax)
                {
                    colMax = mat[r, c];
                }
            }
            Console.WriteLine($"Стовпчик {c}: максимум = {colMax}");
        }
    }
}