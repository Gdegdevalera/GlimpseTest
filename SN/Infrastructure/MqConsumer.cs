using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SN.Messages;
using SN.Services;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SN.Infrastructure
{
    public class MqConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MqConsumer> _logger;
        private readonly MqConsumerConfig _config;

        public MqConsumer(
            IServiceProvider serviceProvider,
            ILogger<MqConsumer> logger,
            IOptions<MqConsumerConfig> config)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _config = config.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                try
                {
                    await DoWork(stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    return;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while consuming messages");
                }
            }
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            var mqFactory = new ConnectionFactory
            {
                HostName = _config.Hostname,
                UserName = _config.User,
                Password = _config.Password,
            };

            using var connection = mqFactory.PatientlyCreateConnection();
            using var channel = connection.CreateModel();

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                var messageStr = Encoding.UTF8.GetString(e.Body.ToArray());
                _logger.LogInformation("Message received: " + messageStr);
                var message = JsonConvert.DeserializeObject<ImageRecognizedMessage>(messageStr);

                using var scope = _serviceProvider.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<ApplicationService>();
                service.UpdateCounters(message);

                channel.BasicAck(e.DeliveryTag, multiple: false);
            };

            var queue = _config.QueueToConsume;
            channel.QueueDeclare(queue,
              durable: true,
              exclusive: false,
              autoDelete: false);
            channel.BasicConsume(queue, autoAck: false, consumer);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
