using RabbitMQ.Client;
using System;
using System.Threading;

namespace SN
{
    public static class Extensions
    {
        public static IConnection PatientlyCreateConnection(this IConnectionFactory mqFactory)
        {
            var counter = 0;
            var lastException = (Exception)null;

            while (counter++ < 20)
            {
                try
                {
                    return mqFactory.CreateConnection();
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    Console.WriteLine(ex.Message);
                    Thread.Sleep(counter * 1000);
                }
            }

            throw lastException;
        }
    }
}
