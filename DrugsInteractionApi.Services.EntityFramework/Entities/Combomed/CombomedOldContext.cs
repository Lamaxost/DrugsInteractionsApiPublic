using DrugsInteractionsApi.Services.EntityFramework.Entities.Shared;
using Microsoft.EntityFrameworkCore;

namespace DrugsInteractionsApi.Services.EntityFramework.Entities.ComboMed
{
    public class CombomedOldContext : DbContext
    {
        public CombomedOldContext(DbContextOptions<CombomedOldContext> options) : base(options)
        {

        }

        public DbSet<Drug> Drugs { get; set; } = default!;
        
        public DbSet<CombomedInteractionRow> InteractionRows { get; set; } = default!;

        public DbSet<CombomedRawInteraction> RawInteractions { get; set; } = default!;
    }
}
