using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace ML
{
    class Program
    {
        private static readonly string[] _categories = new[] {
            "Bottle", "Glass", "Cocktail", "Main course", "Starter", "Side dish", "Salad"
        };

        static void Main(string[] args)
        {
            Console.WriteLine("ML started");

            try
            {
                var mqFactory = new ConnectionFactory
                {
                    HostName = "rabbitmq",
                    UserName = "test",
                    Password = "test"
                };

                using var connection = CreateConnection(mqFactory);
                using var channel = connection.CreateModel();

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (sender, e) =>
                {
                    var body = e.Body;
                    var strMessage = Encoding.UTF8.GetString(body.ToArray());
                    Console.WriteLine("Message received: " + strMessage);

                    var fileUploadedMessage = JsonConvert.DeserializeObject<ImageUploadedMessage>(strMessage);

                    Thread.Sleep(300 + Math.Abs(new Random().Next() % 700)); // 300-1000 ms

                    var categoriesMessage = JsonConvert.SerializeObject(new ImageRecognizedMessage
                    {
                        ImageId = fileUploadedMessage.ImageId,
                        UploadedOn = fileUploadedMessage.UploadedOn,
                        Categories = new[] { _categories.GetRandomOne(), _categories.GetRandomOne() }
                    });

                    channel.BasicAck(e.DeliveryTag, multiple: false);

                    const string queueName = "ImageRecognized";

                    channel.QueueDeclare(queueName, 
                        durable: true, 
                        exclusive: false, 
                        autoDelete: false);

                    channel.BasicPublish(
                        exchange: string.Empty, // direct exchange
                        routingKey: queueName,
                        body: Encoding.UTF8.GetBytes(categoriesMessage)
                    );
                };

                channel.QueueDeclare("ImageUploaded",
                    durable: true,
                    exclusive: false,
                    autoDelete: false);
                channel.BasicConsume("ImageUploaded", autoAck: false, consumer);

                Thread.Sleep(Timeout.Infinite);
            }
            finally
            {
                Console.WriteLine("ML finished");
            }
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

    public static class Extensions
    {
        static readonly Random _random = new();

        public static string GetRandomOne(this string[] source) 
            => source[Math.Abs(_random.Next() % source.Length)];
    }
}
