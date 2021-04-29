using System;

namespace ML.Messages
{
    public class ImageUploadedMessage : IMessage
    {
        public Guid ImageId { get; set; }

        public DateTimeOffset UploadedOn { get; set; }
    }
}
