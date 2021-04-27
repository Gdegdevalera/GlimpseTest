using System;

namespace ML
{
    public class ImageRecognizedMessage
    {
        public Guid ImageId { get; set; }

        public DateTimeOffset UploadedOn { get; set; }

        public string[] Categories { get; set; }
    }
}
