using System;

namespace OPAM_Lab8
{
    public static class Broken
    {
        // Поломка 1: while (lo < hi) замість <=
        public static int Broken1<T>(T[] a, T target, Func<T, T, int> cmp, out int comparisons)
        {
            comparisons = 0;
            int lo = 0, hi = a.Length - 1;
            while (lo < hi)
            {
                int mid = lo + (hi - lo) / 2;
                comparisons++;
                int res = cmp(a[mid], target);
                if (res == 0) return mid;
                if (res < 0) lo = mid + 1;
                else hi = mid - 1;
            }
            return -1;
        }

        // Поломка 2: lo = mid замість mid + 1 (зациклення)
        public static int Broken2<T>(T[] a, T target, Func<T, T, int> cmp, out int comparisons)
        {
            comparisons = 0;
            int lo = 0, hi = a.Length - 1;
            int steps = 0;
            while (lo <= hi)
            {
                steps++;
                if (steps > 1000) break; // Захист від нескінченного циклу
                int mid = lo + (hi - lo) / 2;
                comparisons++;
                int res = cmp(a[mid], target);
                if (res == 0) return mid;
                if (res < 0) lo = mid; // Поломка!
                else hi = mid - 1;
            }
            return -1;
        }

        // Поломка 3: mid = (lo + hi) / 2 (переповнення int при великих ло/хі)
        public static int Broken3<T>(T[] a, T target, Func<T, T, int> cmp, out int comparisons)
        {
            comparisons = 0;
            int lo = 0, hi = a.Length - 1;
            while (lo <= hi)
            {
                int mid = (lo + hi) / 2; // При n=100 000 працює нормально, але ламається при int.MaxValue
                comparisons++;
                int res = cmp(a[mid], target);
                if (res == 0) return mid;
                if (res < 0) lo = mid + 1;
                else hi = mid - 1;
            }
            return -1;
        }

        // Поломка 4: Масив не відсортований
        public static int Broken4<T>(T[] a, T target, Func<T, T, int> cmp, out int comparisons)
        {
            // Працює на невідсортованому масиві
            return Search.BinarySearch(a, target, cmp, out comparisons);
        }
    }
}