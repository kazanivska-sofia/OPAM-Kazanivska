using System;
using System.Collections.Generic;

namespace OPAM_Lab7
{
    public static class Algorithms
    {
        // Завдання 1: Filter<T>
        public static List<T> Filter<T>(List<T> src, Func<T, bool> keep)
        {
            List<T> result = new List<T>();
            if (src == null) return result;

            foreach (var item in src)
            {
                if (keep(item))
                {
                    result.Add(item);
                }
            }
            return result;
        }

        // Завдання 2: Map<Tin, Tout>
        public static List<Tout> Map<Tin, Tout>(List<Tin> src, Func<Tin, Tout> convert)
        {
            List<Tout> result = new List<Tout>();
            if (src == null) return result;

            foreach (var item in src)
            {
                result.Add(convert(item));
            }
            return result;
        }

        // Завдання 3: Aggregate<T, TAcc> (Лише ОДИН алгоритм агрегації на всю роботу)
        public static TAcc Aggregate<T, TAcc>(List<T> src, TAcc start, Func<TAcc, T, TAcc> step)
        {
            TAcc acc = start;
            if (src == null) return acc;

            foreach (var item in src)
            {
                acc = step(acc, item);
            }
            return acc;
        }

        // Завдання 4: GroupBy<TKey, T>
        public static Dictionary<TKey, List<T>> GroupBy<TKey, T>(List<T> src, Func<T, TKey> keyOf)
        {
            Dictionary<TKey, List<T>> result = new Dictionary<TKey, List<T>>();
            if (src == null) return result;

            foreach (var item in src)
            {
                TKey key = keyOf(item);
                if (!result.ContainsKey(key))
                {
                    result[key] = new List<T>();
                }
                result[key].Add(item);
            }
            return result;
        }
    }
}