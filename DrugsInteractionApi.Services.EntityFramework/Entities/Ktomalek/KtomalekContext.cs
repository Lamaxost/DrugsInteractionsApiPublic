using Microsoft.EntityFrameworkCore;

namespace DrugsInteractionApi.Services.EntityFramework.Entities.Ktomalek
{
    public class KtomalekContext : DbContext
    {
        public KtomalekContext(DbContextOptions<KtomalekContext> options) : base(options)
        {
        }

        public DbSet<KtomalekDrug> Drugs => Set<KtomalekDrug>();

        public DbSet<KtomalekInteractionRow> InteractionRows => Set<KtomalekInteractionRow>();
        public DbSet<KtomalekArticle> Articles => Set<KtomalekArticle>();
    }
}

