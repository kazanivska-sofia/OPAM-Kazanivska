using System;
using System.Collections.Generic;

namespace OPAM_Lab9
{
    public static class Sorts
    {
        // Перевірка 1: Чи відсортований масив?
        public static bool IsSorted<T>(T[] a, Func<T, T, int> cmp)
        {
            if (a == null || a.Length <= 1) return true;
            for (int i = 0; i < a.Length - 1; i++)
            {
                if (cmp(a[i], a[i + 1]) > 0) return false;
            }
            return true;
        }

        // Перевірка 2: Чи є відсортований масив перестановкою початкового?
        public static bool IsPermutationOf<T>(T[] original, T[] sorted)
        {
            if (original.Length != sorted.Length) return false;
            Dictionary<T, int> counts = new Dictionary<T, int>();

            foreach (var item in original)
            {
                if (counts.ContainsKey(item)) counts[item]++;
                else counts[item] = 1;
            }

            foreach (var item in sorted)
            {
                if (!counts.ContainsKey(item) || counts[item] == 0) return false;
                counts[item]--;
            }

            return true;
        }

        // Завдання 1: a = 2 (Сортування бульбашкою з прапорцем)
        public static void BubbleSortWithFlag<T>(T[] a, Func<T, T, int> cmp, out long comparisons, out long moves)
        {
            comparisons = 0;
            moves = 0;
            int n = a.Length;
            bool swapped;

            for (int i = 0; i < n - 1; i++)
            {
                swapped = false;
                for (int j = 0; j < n - 1 - i; j++)
                {
                    comparisons++;
                    if (cmp(a[j], a[j + 1]) > 0)
                    {
                        T temp = a[j];
                        a[j] = a[j + 1];
                        a[j + 1] = temp;
                        moves += 3;
                        swapped = true;
                    }
                }
                if (!swapped) break;
            }
        }

        // Завдання 2: b = 3 (QuickSort із медіаною трьох)
        public static void QuickSortMedianOfThree<T>(T[] a, Func<T, T, int> cmp, out long comparisons, out long moves)
        {
            comparisons = 0;
            moves = 0;
            if (a == null || a.Length <= 1) return;
            QuickSortRec(a, 0, a.Length - 1, cmp, ref comparisons, ref moves);
        }

        private static void QuickSortRec<T>(T[] a, int low, int high, Func<T, T, int> cmp, ref long comparisons, ref long moves)
        {
            if (low < high)
            {
                int pivotIdx = Partition(a, low, high, cmp, ref comparisons, ref moves);
                QuickSortRec(a, low, pivotIdx - 1, cmp, ref comparisons, ref moves);
                QuickSortRec(a, pivotIdx + 1, high, cmp, ref comparisons, ref moves);
            }
        }

        private static int Partition<T>(T[] a, int low, int high, Func<T, T, int> cmp, ref long comparisons, ref long moves)
        {
            int mid = low + (high - low) / 2;
            
            // Вибір медіани трьох (low, mid, high)
            int pivotIndex = MedianOfThreeIndex(a, low, mid, high, cmp, ref comparisons);
            
            // Міняємо опорний елемент з останнім
            T pTemp = a[pivotIndex];
            a[pivotIndex] = a[high];
            a[high] = pTemp;
            moves += 3;

            T pivot = a[high];
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                comparisons++;
                if (cmp(a[j], pivot) <= 0)
                {
                    i++;
                    T temp = a[i];
                    a[i] = a[j];
                    a[j] = temp;
                    moves += 3;
                }
            }

            T finalTemp = a[i + 1];
            a[i + 1] = a[high];
            a[high] = finalTemp;
            moves += 3;

            return i + 1;
        }

        private static int MedianOfThreeIndex<T>(T[] a, int i1, int i2, int i3, Func<T, T, int> cmp, ref long comparisons)
        {
            comparisons++;
            if (cmp(a[i1], a[i2]) > 0)
            {
                comparisons++;
                if (cmp(a[i2], a[i3]) > 0) return i2;
                comparisons++;
                return cmp(a[i1], a[i3]) > 0 ? i3 : i1;
            }
            else
            {
                comparisons++;
                if (cmp(a[i1], a[i3]) > 0) return i1;
                comparisons++;
                return cmp(a[i2], a[i3]) > 0 ? i3 : i2;
            }
        }

        // Завдання 3: c = 1 (Сортування вставками з бінарним пошуком позиції)
        public static void BinaryInsertionSort<T>(T[] a, Func<T, T, int> cmp, out long comparisons, out long moves)
        {
            comparisons = 0;
            moves = 0;

            for (int i = 1; i < a.Length; i++)
            {
                T key = a[i];
                moves++;

                // Бінарний пошук позиції для вставки
                int lo = 0;
                int hi = i;
                while (lo < hi)
                {
                    int mid = lo + (hi - lo) / 2;
                    comparisons++;
                    if (cmp(key, a[mid]) < 0)
                    {
                        hi = mid;
                    }
                    else
                    {
                        lo = mid + 1;
                    }
                }

                // Зсув елементів праворуч
                for (int j = i; j > lo; j--)
                {
                    a[j] = a[j - 1];
                    moves++;
                }
                a[lo] = key;
                moves++;
            }
        }
    }
}