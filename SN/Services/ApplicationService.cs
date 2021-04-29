using SN.Data;
using SN.Messages;
using System;
using System.Linq;

namespace SN.Services
{
    public class ApplicationService
    {
        private readonly ApplicationDbContext _db;

        public ApplicationService(ApplicationDbContext db)
        {
            _db = db;
        }

        public void UpdateCounters(ImageRecognizedMessage message)
        {
            foreach (var category in message.Categories)
            {
                using var transaction = _db.Database.BeginTransaction();
                var hour = RoundToHours(message.UploadedOn);

                var entityByHour = _db.Categories
                    .FirstOrDefault(x => x.Name == category && x.Hour == hour);

                if (entityByHour == null)
                {
                    _db.Categories.Add(new Category
                    {
                        Name = category,
                        Total = 1,
                        Hour = hour
                    });
                }
                else
                {
                    entityByHour.Total++;
                }

                _db.SaveChanges();
                transaction.Commit();
            }
        }

        private static DateTimeOffset RoundToHours(DateTimeOffset source)
        {
            return new DateTimeOffset(
                source.Year, source.Month, source.Day,
                source.Hour, 0, 0,
                source.Offset);
        }
    }
}
