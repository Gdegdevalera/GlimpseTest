using System;
using System.Threading;

namespace ML
{
    public static class TestHelper
    {
        static readonly Random _random = new();

        public static string GetRandomOne(this string[] source)
            => source[_random.Next() % source.Length];

        public static void Delay()
        {
            var min = Environment.GetEnvironmentVariable("Test:Delay:Min");
            var max = Environment.GetEnvironmentVariable("Test:Delay:Max");

            if (min == null || max == null)
                return;

            var minNum = int.Parse(min);
            var maxNum = int.Parse(max);

            Thread.Sleep(minNum + _random.Next() % (maxNum - minNum));
        }
    }
}
