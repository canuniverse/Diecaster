using System.Collections.Generic;
using System.Linq;

namespace DeepSlay
{
    public static class Extensions
    {
        public static T Random<T>(this List<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }
        
        public static List<T> Randomize<T>(this IEnumerable<T> list)
        {
            return list.OrderBy(_ => UnityEngine.Random.value).ToList();
        }
        
        public static void AddNew<T>(this List<T> list, T item)
        {
            if (!list.Contains(item))
            {
                list.Add(item);
            }
        }

        public static void AddNew<T>(this List<T> list, List<T> addition)
        {
            foreach (var item in addition)
            {
                if (!list.Contains(item))
                {
                    list.Add(item);
                }
            }
        }
    }
}