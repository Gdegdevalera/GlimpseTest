using System;
using System.IO;
using System.Linq;

namespace ML.Services
{
    public interface IStorage
    {
        byte[] ReadFile(Guid imageId);
    }

    public class Storage : IStorage
    {
        private readonly string _storagePath;

        public Storage(string storagePath)
        {
            _storagePath = storagePath;
        }

        public byte[] ReadFile(Guid imageId)
        {
            try
            {
                var id = imageId.ToString();
                var fileName = Directory
                    .EnumerateFiles(_storagePath)
                    .FirstOrDefault(x => x.Contains(id));

                if (fileName == null)
                {
                    Console.WriteLine($"Error! File id: {id} is missing");
                    return null;
                }

                return File.ReadAllBytes(fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
