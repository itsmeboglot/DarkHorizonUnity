using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Core.Helpers
{
    public static class CSharpExtension
    {
        public static T[] Add<T>(this T[] target, params T[] items)
        {
            // Validate the parameters
            if (target == null)
                return items;

            if (items == null)
                return target;

            // Join the arrays
            var result = new T[target.Length + items.Length];
            target.CopyTo(result, 0);
            items.CopyTo(result, target.Length);
            return result;
        }

        public static IEnumerable<T> Remove<T>(this IEnumerable<T> array, int index)
        {
            return array.Where((val, idx) => idx != index);
        }

        public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
                dictionary[key] = value;
            else
                dictionary.Add(key, value);
        }

        /// <summary>
        /// Returns a random long from min (inclusive) to max (exclusive)
        /// </summary>
        /// <param name="random">The given random instance</param>
        /// <param name="min">The inclusive minimum bound</param>
        /// <param name="max">The exclusive maximum bound.  Must be greater than min</param>
        public static long Next(this Random random, long min, long max)
        {
            if (max <= min)
                throw new ArgumentOutOfRangeException("max", "max must be > min!");

            //Working with ulong so that modulo works correctly with values > long.MaxValue
            ulong uRange = (ulong)(max - min);

            //Prevent a modolo bias; see https://stackoverflow.com/a/10984975/238419
            //for more information.
            //In the worst case, the expected number of calls is 2 (though usually it's
            //much closer to 1) so this loop doesn't really hurt performance at all.
            ulong ulongRand;
            do
            {
                var buf = new byte[8];
                random.NextBytes(buf);
                ulongRand = (ulong)BitConverter.ToInt64(buf, 0);
            } while (ulongRand > ulong.MaxValue - ((ulong.MaxValue % uRange) + 1) % uRange);

            return (long)(ulongRand % uRange) + min;
        }
        
        /// <summary>
        /// Returns a random long from 0 (inclusive) to max (exclusive)
        /// </summary>
        /// <param name="random">The given random instance</param>
        /// <param name="max">The exclusive maximum bound.  Must be greater than 0</param>
        public static long Next(this Random random, long max)
        {
            return random.Next(0, max);
        }

        /// <summary>
        /// Returns a random long over all possible values of long (except long.MaxValue, similar to
        /// random.Next())
        /// </summary>
        /// <param name="random">The given random instance</param>
        public static long NextLong(this Random random)
        {
            return random.Next(long.MinValue, long.MaxValue);
        }
    }

    public static class Add<T>
    {
        public static readonly Func<T, T, T> Do;

        static Add()
        {
            var par1 = Expression.Parameter(typeof(T));
            var par2 = Expression.Parameter(typeof(T));

            var add = Expression.Add(par1, par2);

            Do = Expression.Lambda<Func<T, T, T>>(add, par1, par2).Compile();
        }
    }

    public static class Scale<T>
    {
        public static readonly Func<T, T, T> Do;

        static Scale()
        {
            var par1 = Expression.Parameter(typeof(T));
            var par2 = Expression.Parameter(typeof(double));

            try
            {
                Do = Expression
                    .Lambda<Func<T, T, T>>(
                        Expression.Multiply(par1, par2),
                        par1, par2)
                    .Compile();
            }
            catch
            {
                Do = Expression
                    .Lambda<Func<T, T, T>>(
                        Expression.Convert(
                            Expression.Multiply(
                                Expression.Convert(par1, typeof(double)),
                                par2),
                            typeof(T)),
                        par1, par2)
                    .Compile();
            }
        }
    }
}