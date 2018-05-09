using System;
using System.Collections.Generic;

namespace Task1
{
    internal static class EnumerableExtensions
    {
        public static T MaxBy<T, TR>(this IEnumerable<T> container, Func<T, TR> valuingFoo) where TR : IComparable
        {
            var maxElement = default(T);
            using (var enumerator = container.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    throw new ArgumentException("Container is empty!");

                maxElement = enumerator.Current;
                var maxValue = valuingFoo(maxElement);

                while (enumerator.MoveNext())
                {
                    var currentValue = valuingFoo(enumerator.Current);

                    if (currentValue.CompareTo(maxValue) > 0)
                    {
                        maxValue = currentValue;
                        maxElement = enumerator.Current;
                    }
                }
            }
            return maxElement;
        }
    }
}