using Microsoft.EntityFrameworkCore;

namespace DrugsInteractionApi.Services.EntityFramework.Entities.NotFound
{
    public class NotFoundContext : DbContext
    {
        public NotFoundContext(DbContextOptions<NotFoundContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<NotFoundMessage> Messages { get; set; }
    }
}
