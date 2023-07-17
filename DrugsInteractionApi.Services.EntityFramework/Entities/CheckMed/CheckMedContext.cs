using Microsoft.EntityFrameworkCore;
using DrugsInteractionsApi.Services.EntityFramework.Entities.Shared;

namespace DrugsInteractionsApi.Services.EntityFramework.Entities.CheckMed
{
    public class CheckMedContext : DbContext
    {
        public CheckMedContext(DbContextOptions<CheckMedContext> options) : base(options)
        {

        }
        public virtual DbSet<Drug> Drugs => Set<Drug>();
        public DbSet<CheckMedInteractionRow> InteractionRows => Set<CheckMedInteractionRow>(); 
    }
}
