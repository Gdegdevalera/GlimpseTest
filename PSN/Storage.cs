using System;
using System.IO;
using System.Threading.Tasks;

namespace PSN
{
    public class Storage
    {
        private readonly string _storagePath;

        public Storage(string storagePath)
        {
            _storagePath = storagePath;
        }

        public async Task<(Guid, DateTimeOffset)> Save(Stream requestStream)
        {
            var now = DateTimeOffset.UtcNow;
            var fileId = Guid.NewGuid();
            var fileName = now.ToString("yyyy_MM_dd__HH_mm_ss_ffff___") + fileId;
            var filePath = Path.Combine(_storagePath, fileName);

            using var file = File.Create(filePath);
            await requestStream.CopyToAsync(file);
            return (fileId, now);
        }
    }
}
