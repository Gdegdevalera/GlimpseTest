using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SN.Data;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SN.Services
{
    public class MqConsumer : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceProvider _serviceProvider;

        public MqConsumer(IServiceProvider serviceProvider)
        {
            var factory = new ConnectionFactory
            {
                HostName = "rabbitmq",
                UserName = "test",
                Password = "test"
            };

            _connection = CreateConnection(factory);
            _channel = _connection.CreateModel();
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (sender, e) =>
            {
                Console.WriteLine("Processing started");

                var strMessage = Encoding.UTF8.GetString(e.Body.ToArray());
                var imageRecognizedMessage = JsonConvert.DeserializeObject<ImageRecognizedMessage>(strMessage);

                using var scope = _serviceProvider.CreateScope();
                using var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                using var transaction = db.Database.BeginTransaction();

                foreach (var category in imageRecognizedMessage.Categories)
                {
                    var entity = db.Categories.FirstOrDefault(x => x.Name == category);
                    if (entity == null)
                    {
                        db.Categories.Add(new Category { Name = category, Total = 1 });
                    }
                    else
                    {
                        entity.Total++;
                    }

                    var hour = imageRecognizedMessage.UploadedOn.RoundToHours();
                    var entityByHour = db.CategoriesByHour.FirstOrDefault(x => x.Hour == hour);
                    if (entityByHour == null)
                    {
                        db.CategoriesByHour.Add(new CategoryByHour { Name = category, Total = 1, Hour = hour });
                    }
                    else
                    {
                        entityByHour.Total++;
                    }
                }

                db.SaveChanges();
                transaction.Commit();

                _channel.BasicAck(e.DeliveryTag, multiple: false);
                Console.WriteLine("Processing finished");
            };

            _channel.QueueDeclare("ImageRecognized",
              durable: true,
              exclusive: false,
              autoDelete: false);
            _channel.BasicConsume("ImageRecognized", autoAck: false, consumer);

            return Task.Delay(Timeout.Infinite, stoppingToken);
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
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
        public static DateTimeOffset RoundToHours(this DateTimeOffset source)
        {
            return new DateTimeOffset(
                source.Year, source.Month, source.Day, 
                source.Hour, source.Minute, 0, // TODO set minutes to 0
                source.Offset);
        }
    }
}
