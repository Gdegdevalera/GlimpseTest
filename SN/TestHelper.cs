using System;
using System.Threading;

namespace SN
{
    public static class TestHelper
    {
        static readonly Random _random = new();

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

        public static void SetThreadPoolSize()
        {
            var sizeStr = Environment.GetEnvironmentVariable("Test:ThreadPool:Size");

            if (sizeStr == null)
                return;

            // size must be greater or equal to processor cores count
            var size = int.Parse(sizeStr);

            var changed = ThreadPool.SetMaxThreads(size, size);

            if (!changed)
                throw new Exception("Invalid thread pool size!");
        }
    }
}
