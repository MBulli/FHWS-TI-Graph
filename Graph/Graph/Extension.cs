using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    static class Extension
    {
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

        public static void Push<T>(this Stack<T> stack, IEnumerable<T> items)
        {
            foreach (var item in items)
                stack.Push(item);
        }

        public static IDictionary<T, C5.IPriorityQueueHandle<T>> AddRange<T>(this C5.IPriorityQueue<T> heap, IEnumerable<T> elements)
        {
            var dict = new Dictionary<T, C5.IPriorityQueueHandle<T>>();

            foreach (var item in elements)
            {
                C5.IPriorityQueueHandle<T> handle = null;

                if (heap.Add(ref handle, item))
                {
                    dict[item] = handle;
                }
            }

            return dict;
        }
    }
}
