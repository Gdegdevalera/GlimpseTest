using System.ComponentModel.DataAnnotations;

namespace SN.Infrastructure
{
    public class MqConsumerConfig
    {
        [Required, MinLength(1)]
        public string Hostname { get; set; }

        [Required, MinLength(1)]
        public string User { get; set; }

        [Required, MinLength(1)]
        public string Password { get; set; }

        [Required, MinLength(1)]
        public string QueueToConsume { get; set; }
    }
}
