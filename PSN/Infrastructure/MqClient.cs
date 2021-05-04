using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PSN.Messages;
using RabbitMQ.Client;
using System;
using System.Text;

namespace PSN.Infrastructure
{
    public interface IMqClient
    {
        void Publish(ImageUploadedMessage message);
    }

    public class MqClient : IMqClient, IDisposable
    {
        private readonly string _queueName;

        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MqClient(IOptions<RabbitMqConfig> options)
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = options.Value.Hostname,
                UserName = options.Value.User,
                Password = options.Value.Password,
            };

            _queueName = options.Value.QueueToPublish;

            _connection = connectionFactory.PatientlyCreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(_queueName,
                durable: true,
                exclusive: false,
                autoDelete: false);
        }

        public void Publish(ImageUploadedMessage message)
        {
            _channel.BasicPublish(
                exchange: string.Empty, // direct exchange
                routingKey: _queueName,
                body: Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)));
        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }
    }
}
