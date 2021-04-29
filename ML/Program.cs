using ML.Messages;
using ML.Services;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace ML
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ML node started");

            var configuration = new MLConfiguration();

            var service = new EventsService(
                new Recognizer(),
                new Storage(configuration.StoragePath));

            var mqFactory = new ConnectionFactory
            {
                HostName = configuration.MqHostname,
                UserName = configuration.MqUserName,
                Password = configuration.MqPassword,
            };

            using var connection = mqFactory.PatientlyCreateConnection();
            using var channel = connection.CreateModel();
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (sender, e) =>
            {
                try
                {
                    var strMessage = Encoding.UTF8.GetString(e.Body.ToArray());
                    Console.WriteLine("Message received: " + strMessage);
                    var request = JsonConvert.DeserializeObject<ImageUploadedMessage>(strMessage);

                    var response = service.OnImageUploaded(request);
                    if (response == null)
                        return;

                    var responseStr = JsonConvert.SerializeObject(response);
                    Console.WriteLine("Sending message: " + responseStr);
                    channel.BasicAck(e.DeliveryTag, multiple: false);

                    channel.QueueDeclare(configuration.QueueToPublish,
                        durable: true,
                        exclusive: false,
                        autoDelete: false);

                    channel.BasicPublish(
                        exchange: string.Empty, // direct exchange
                        routingKey: configuration.QueueToPublish,
                        body: Encoding.UTF8.GetBytes(responseStr)
                    );
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            };

            // setup equal pressure to each node
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            channel.QueueDeclare(configuration.QueueToConsume,
                durable: true,
                exclusive: false,
                autoDelete: false);
            channel.BasicConsume(configuration.QueueToConsume, autoAck: false, consumer);

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
