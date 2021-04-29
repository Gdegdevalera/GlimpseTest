using System;

namespace ML
{
    partial class Program
    {
        class MLConfiguration
        {
            public string StoragePath { get; } = Get("Storage:Path");
            public string MqHostname { get; } = Get("RabbitMq:Hostname");
            public string MqUserName { get; } = Get("RabbitMq:User");
            public string MqPassword { get; } = Get("RabbitMq:Password");
            public string QueueToConsume { get; } = Get("RabbitMq:QueueToConsume");
            public string QueueToPublish { get; } = Get("RabbitMq:QueueToPublish");

            private static string Get(string name)
            {
                var value = Environment.GetEnvironmentVariable(name);

                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception($"EnvironmentVariable {name} is missing!");

                return value;
            }
        }
    }
}
