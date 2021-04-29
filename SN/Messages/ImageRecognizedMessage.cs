using System;

namespace SN.Messages
{
    public class ImageRecognizedMessage
    {
        public Guid ImageId { get; set; }

        public DateTimeOffset UploadedOn { get; set; }

        public string[] Categories { get; set; }
    }
}
