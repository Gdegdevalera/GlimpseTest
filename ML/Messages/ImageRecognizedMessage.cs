using System;

namespace ML.Messages
{
    public class ImageRecognizedMessage : IMessage
    {
        public Guid ImageId { get; set; }

        public DateTimeOffset UploadedOn { get; set; }

        public string[] Categories { get; set; }
    }
}
