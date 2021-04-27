using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;

namespace PSN
{

    public class Queue
    {
        private readonly ConnectionFactory _mqFactory;

        public Queue()
        {
            _mqFactory = new ConnectionFactory
            {
                HostName = "rabbitmq",
                UserName = "test",
                Password = "test"
            };
        }

        public void Publish(ImageUploadedMessage message)
        {
            using var connection = CreateConnection(_mqFactory);
            using var channel = connection.CreateModel();
            const string queueName = "ImageUploaded";

            channel.QueueDeclare(queueName, 
                durable: true, 
                exclusive: false,
                autoDelete: false);

            channel.BasicPublish(
                exchange: string.Empty, // direct exchange
                routingKey: queueName,
                body: Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)));
        }

        private static IConnection CreateConnection(IConnectionFactory mqFactory)
        {
            var counter = 0;
            var lastException = (Exception)null;

            while (counter++ < 10)
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
