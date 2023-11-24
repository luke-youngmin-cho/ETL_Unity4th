using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionMethod
{
    internal static class FakeEnumerable
    {
        public static IEnumerable<T> Where<T>(this IEnumerable<T> source, Predicate<T> predicate)
        {
            List<T> list = new List<T>();
            foreach (T item in source)
                if (predicate(item))
                    list.Add(item);
            return list;
        }
    }
}
