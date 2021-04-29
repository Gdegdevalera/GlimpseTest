using System;

namespace PSN.Messages
{
    public class ImageUploadedMessage
    {
        public Guid ImageId { get; set; }

        public DateTimeOffset UploadedOn { get; set; }
    }
}
