using SN.Data;
using SN.Messages;
using SN.Models;
using System;
using System.Linq;

namespace SN.Services
{
    public class ApplicationService
    {
        private readonly IApplicationDbContext _db;

        public ApplicationService(IApplicationDbContext db)
        {
            _db = db;
        }

        public CategoriesViewModel GetCategories()
        {
            var now = DateTimeOffset.UtcNow.AddDays(-1);

            return new CategoriesViewModel
            {
                Categories = _db.Categories.Where(x => x.Hour >= now)
            };
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
