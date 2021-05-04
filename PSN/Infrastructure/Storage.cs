using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PSN.Infrastructure
{
    public interface IStorage
    {
        Task<(Guid, DateTimeOffset)> Save(Stream requestStream);
    }

    public class Storage : IStorage
    {
        private readonly string _storagePath;

        public Storage(IOptions<StorageConfig> config)
        {
            _storagePath = config.Value.Path;
        }

        public async Task<(Guid, DateTimeOffset)> Save(Stream requestStream)
        {
            var now = DateTimeOffset.UtcNow;

            TestHelper.Delay();

            var fileId = Guid.NewGuid();
            var fileName = now.ToString("yyyy_MM_dd__HH_mm_ss_ffff___") + fileId;
            var filePath = Path.Combine(_storagePath, fileName);

            using var file = File.Create(filePath);
            await requestStream.CopyToAsync(file);
            return (fileId, now);
        }
    }
}
