using System;

namespace SN.Services
{
    public class ImageRecognizedMessage
    {
        public Guid ImageId { get; set; }

        public DateTimeOffset UploadedOn { get; set; }

        public string[] Categories { get; set; }
    }
}
