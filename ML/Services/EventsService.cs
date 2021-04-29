using ML.Messages;

namespace ML.Services
{
    public class EventsService
    {
        private readonly IRecognizer _recognizer;
        private readonly IStorage _storage;

        public EventsService(
            IRecognizer recognizer,
            IStorage storage)
        {
            _recognizer = recognizer;
            _storage = storage;
        }

        public ImageRecognizedMessage OnImageUploaded(ImageUploadedMessage message)
        {
            var bytes = _storage.ReadFile(message.ImageId);

            if (bytes == null)
                return null;

            return new ImageRecognizedMessage
            {
                ImageId = message.ImageId,
                UploadedOn = message.UploadedOn,
                Categories = _recognizer.GetCategories(bytes)
            };
        }
    }
}
