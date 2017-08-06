using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{

    public static class MyLinq
    {
        public static IEnumerable<T> Where<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            foreach (var item in source)
            {
                if (predicate(item))
                {
                    yield return item;
                }
            }
        }
        public static IEnumerable<T2> Select<T1, T2>(this IEnumerable<T1> source, Func<T1, T2> predicate)
        {
            foreach (var item in source)
            {
                yield return predicate(item);
            }
        }
        public static bool Any<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            return source.Where(predicate).GetEnumerator().MoveNext();
        }
        public static bool Any<T>(this IEnumerable<T> source)
        {
            return source.GetEnumerator().MoveNext();
        }
        public static T1 Sum<T1>(this IEnumerable<T1> source, Func<T1, T1, T1> func)
        {
            var enumerator = source.GetEnumerator();
            enumerator.MoveNext();
            var sum = enumerator.Current;
            while (enumerator.MoveNext())
            {
                sum = func(sum, enumerator.Current);
            }
            return sum;
        }
        public static bool CountAny<T>(this IEnumerable<T> source, int count, Func<T, bool> condition)
        {
            int found = 0;
            while (source.Any() && (found < count))
            {
                source = source.SkipWhile(n => !condition(n));
                source = source.Skip(1);
                found++;
            }
            return source.Any();
        }
        public static bool CountAny2<T>(this IEnumerable<T> source, int count, Func<T, bool> condition)
        {
            int found = 0;
            foreach (var item in source)
            {
                if (condition(item))
                {
                    found++;
                }
                if (found >= count)
                {
                    break;
                }
            }
            return found >= count;
        }
    }
    class Program
    {

        static void Main(string[] args)
        {
            //var source = GetSourceList();

            //var data2 = source.Select((i, index) => i);
            //foreach (var item in data2)
            //{
            //    Console.WriteLine(item);
            //}

            //string sum = data2.Sum((s, i) => s + "," + i);
            //Console.WriteLine(sum);

            //var source = Enumerable.Range(0, 500000000);
            //Stopwatch sw = Stopwatch.StartNew();
            //bool aa = source.Skip(3).Any();
            //sw.Stop();
            //Console.WriteLine(sw.ElapsedMilliseconds);

            var source = Enumerable.Range(0, 500000000);
            Stopwatch sw = Stopwatch.StartNew();
            var aa = source.CountAny(3, n => n % 3 == 0);
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);

            sw.Restart();
            var bb=source.CountAny2(3, n => n % 3 == 0);
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);

            sw.Restart();
            var cc = source.Where( n => n % 3 == 0).Count()>3;
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);

            Console.WriteLine("aa:"+aa);
            Console.WriteLine("bb:"+bb);
            Console.ReadKey();
        }
        public static IEnumerable<string> GetSource()
        {
            var length = 10;
            for (int i = 0; i < length; i++)
            {
                yield return i.ToString();
            }
        }
        public static IEnumerable<string> GetSourceList()
        {
            var list = new List<string>();
            var length = 10;
            for (int i = 0; i < length; i++)
            {
                list.Add(i.ToString());
            }
            return list;
        }
    }
}
