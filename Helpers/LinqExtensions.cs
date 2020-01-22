using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides a set of static methods for querying objects that 
    /// implement <see cref="IEnumerable{T}" />. The actual methods
    /// are implemented in files reflecting the method name.
    /// </summary>

    public static partial class MoreEnumerable { }

    static partial class MoreEnumerable
    {
        private static Lazy<Random> _defaultRng = new Lazy<Random>(() => new Random());

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable, Random rng = null)
        {
            if (!enumerable.Any())
                yield break;

            var list = enumerable.ToList();
            rng = rng ?? _defaultRng.Value;

            for (int i = list.Count - 1; i >= 0; i--)
            {
                int next = rng.Next(i + 1);
                yield return list[next];
                list[next] = list[i];
            }
        }
    }

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Skips items from the input sequence until the given predicate returns true
        /// when applied to the current source item; that item will be the last skipped.
        /// </summary>
        /// <remarks>
        /// <para>
        /// SkipUntil differs from Enumerable.SkipWhile in two respects. Firstly, the sense
        /// of the predicate is reversed: it is expected that the predicate will return false
        /// to start with, and then return true - for example, when trying to find a matching
        /// item in a sequence.
        /// </para>
        /// <para>
        /// Secondly, SkipUntil skips the element which causes the predicate to return true. For
        /// example, in a sequence <code>{ 1, 2, 3, 4, 5 }</code> and with a predicate of
        /// <code>x => x == 3</code>, the result would be <code>{ 4, 5 }</code>.
        /// </para>
        /// <para>
        /// SkipUntil is as lazy as possible: it will not iterate over the source sequence
        /// until it has to, it won't iterate further than it has to, and it won't evaluate
        /// the predicate until it has to. (This means that an item may be returned which would
        /// actually cause the predicate to throw an exception if it were evaluated, so long as
        /// it comes after the first item causing the predicate to return true.)
        /// </para>
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="predicate">Predicate used to determine when to stop yielding results from the source.</param>
        /// <returns>Items from the source sequence after the predicate first returns true when applied to the item.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null</exception>

        public static IEnumerable<TSource> SkipUntil<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (predicate == null) throw new ArgumentNullException("predicate");
            return SkipUntilImpl(source, predicate);
        }

        private static IEnumerable<TSource> SkipUntilImpl<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            using (var iterator = source.GetEnumerator())
            {
                while (iterator.MoveNext())
                {
                    if (predicate(iterator.Current))
                    {
                        break;
                    }
                }
                while (iterator.MoveNext())
                {
                    yield return iterator.Current;
                }
            }
        }
    }
    static partial class MoreEnumerable
    {
        /// <summary>
        /// Returns items from the input sequence until the given predicate returns true
        /// when applied to the current source item; that item will be the last returned.
        /// </summary>
        /// <remarks>
        /// <para>
        /// TakeUntil differs from Enumerable.TakeWhile in two respects. Firstly, the sense
        /// of the predicate is reversed: it is expected that the predicate will return false
        /// to start with, and then return true - for example, when trying to find a matching
        /// item in a sequence.
        /// </para>
        /// <para>
        /// Secondly, TakeUntil yields the element which causes the predicate to return true. For
        /// example, in a sequence <code>{ 1, 2, 3, 4, 5 }</code> and with a predicate of
        /// <code>x => x == 3</code>, the result would be <code>{ 1, 2, 3 }</code>.
        /// </para>
        /// <para>
        /// TakeUntil is as lazy as possible: it will not iterate over the source sequence
        /// until it has to, it won't iterate further than it has to, and it won't evaluate
        /// the predicate until it has to. (This means that an item may be returned which would
        /// actually cause the predicate to throw an exception if it were evaluated, so long as
        /// no more items of data are requested.)
        /// </para>
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="predicate">Predicate used to determine when to stop yielding results from the source.</param>
        /// <returns>Items from the source sequence, until the predicate returns true when applied to the item.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null</exception>

        public static IEnumerable<TSource> TakeUntil<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (predicate == null) throw new ArgumentNullException("predicate");
            return TakeUntilImpl(source, predicate);
        }

        private static IEnumerable<TSource> TakeUntilImpl<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            foreach (var item in source)
            {
                yield return item;
                if (predicate(item))
                {
                    yield break;
                }
            }
        }
    }
}
