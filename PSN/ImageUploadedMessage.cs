using System;

namespace PSN
{
    public class ImageUploadedMessage
    {
        public Guid ImageId { get; set; }

        public DateTimeOffset UploadedOn { get; set; }
    }
}
