using PSN.Infrastructure;
using PSN.Messages;
using System.IO;
using System.Threading.Tasks;

namespace PSN.Services
{
    public class RequestHandler
    {
        private readonly IStorage _storage;
        private readonly IMqClient _mqClient;

        public RequestHandler(IStorage storage, IMqClient mqClient)
        {
            _storage = storage;
            _mqClient = mqClient;
        }

        public async Task<ImageUploadedMessage> Handle(Stream imageStream)
        {
            var (imageId, uploadedOn) = await _storage.Save(imageStream);

            var message = new ImageUploadedMessage
            {
                ImageId = imageId,
                UploadedOn = uploadedOn
            };

            _mqClient.Publish(message);
            return message;
        }
    }
}
