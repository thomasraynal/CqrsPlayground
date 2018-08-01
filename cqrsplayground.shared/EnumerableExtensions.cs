using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cqrsplayground.shared
{
    public static class EnumerableExtensions
    {
        private static Random _rand = new Random();

        public static T Random<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }


            var list = enumerable as IList<T> ?? enumerable.ToList();
            return list.Count == 0 ? default(T) : list[_rand.Next(0, list.Count)];
        }

    }
}
