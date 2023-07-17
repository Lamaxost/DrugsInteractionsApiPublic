using DrugsInteractionsApi.Services.EntityFramework.Entities.Shared;
using Microsoft.EntityFrameworkCore;

namespace DrugsInteractionApi.Services.EntityFramework.Entities.DrugsCom.OldBase
{
    public class DrugsComContext:DbContext
    {
        public DrugsComContext(DbContextOptions<DrugsComContext> options) : base(options)
        {

        }

        public DbSet<DrugsComDrug> Drugs => Set<DrugsComDrug>();
        public DbSet<DrugsInteractionRow> InteractionRows => Set<DrugsInteractionRow>();
    }
}
