using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    static class Extension
    {
        static Random randomGenerator = new Random();

        public static StringBuilder AppendLineFormat(this StringBuilder sb, string format, params object[] args)
        {
            sb.AppendFormat(format, args);
            sb.AppendLine();

            return sb;
        }

        public static void ForEach<T>(this IEnumerable<T> e, Action<T> action)
        {
            foreach (T item in e)
            {
                action(item);
            }
        }

        public static T MaxOrDefault<T>(this IEnumerable<T> e, T defaultValue = default(T))
        {
            return e.DefaultIfEmpty(defaultValue).Max();
        }

        public static T MinOrDefault<T>(this IEnumerable<T> e, T defaultValue = default(T))
        {
            return e.DefaultIfEmpty(defaultValue).Min();
        }

        public static void Push<T>(this Stack<T> stack, IEnumerable<T> items)
        {
            foreach (var item in items)
                stack.Push(item);
        }

        public static T RandomElement<T>(this IEnumerable<T> e)
        {
            var generator = new Random();
            var randIndex = generator.Next(0, e.Count());
            return e.ElementAt(randIndex);
        }

        public static T RandomElementOrDefault<T>(this IEnumerable<T> e, T defaultValue = default(T))
        {
            var count = e.Count();

            if (count == 0)
                return defaultValue;

            var randIndex = randomGenerator.Next(0, count);
            return e.ElementAt(randIndex);
        }
    }
}
