using System.ComponentModel.DataAnnotations;

namespace PSN.Infrastructure
{
    public class StorageConfig
    {
        [Required, MinLength(1)]
        public string Path { get; set; }
    }

    public class RabbitMqConfig
    {
        [Required, MinLength(1)]
        public string Hostname { get; set; }

        [Required, MinLength(1)]
        public string User { get; set; }

        [Required, MinLength(1)]
        public string Password { get; set; }

        [Required, MinLength(1)]
        public string QueueToPublish { get; set; }
    }
}
