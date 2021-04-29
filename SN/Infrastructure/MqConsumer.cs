using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
        private readonly IConfiguration _configuration;

        public MqConsumer(
            IServiceProvider serviceProvider,
            ILogger<MqConsumer> logger,
            IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _configuration = configuration;
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
                HostName = _configuration["RabbitMq:Hostname"],
                UserName = _configuration["RabbitMq:User"],
                Password = _configuration["RabbitMq:Password"],
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

            var queue = _configuration["RabbitMq:QueueToConsume"];
            channel.QueueDeclare(queue,
              durable: true,
              exclusive: false,
              autoDelete: false);
            channel.BasicConsume(queue, autoAck: false, consumer);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
