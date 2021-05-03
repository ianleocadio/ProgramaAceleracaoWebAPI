using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<TSource> Intersect<TSource, TSecondSource, TKeySelector>(this IEnumerable<TSource> first,
            IEnumerable<TSecondSource> second, Func<TSource, TKeySelector> firstKeySelector, Func<TSecondSource, TKeySelector> secondKeySelector)
        {
            if (first is null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second is null)
            {
                return new List<TSource>();
            }

            var secondHashSet = new HashSet<TKeySelector>(second.Select(secondKeySelector));
            return first.Where(e => secondHashSet.Contains(firstKeySelector(e)));
        }

        public static IEnumerable<TSource> Except<TSource, TSecondSource, TKeySelector>(this IEnumerable<TSource> first,
            IEnumerable<TSecondSource> second, Func<TSource, TKeySelector> firstKeySelector, Func<TSecondSource, TKeySelector> secondKeySelector)
        {
            if (first is null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second is null)
            {
                return first;
            }

            var secondHashSet = new HashSet<TKeySelector>(second.Select(secondKeySelector));
            return first.Where(e => !secondHashSet.Contains(firstKeySelector(e)));
        }
    }
}
