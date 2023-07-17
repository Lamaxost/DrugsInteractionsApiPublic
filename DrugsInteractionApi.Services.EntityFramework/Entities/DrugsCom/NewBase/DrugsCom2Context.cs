using Microsoft.EntityFrameworkCore;
using DrugsInteractionsApi.Services.EntityFramework.Entities.Shared;

namespace DrugsInteractionApi.Services.EntityFramework.Entities.DrugsCom.NewBase
{
    public class DrugsCom2Context:DbContext
    {
        public DbSet<DrugsCom2Drug> Drugs => Set<DrugsCom2Drug>();

        public DbSet<RawDrugCom2Interaction> RawInteractions => Set<RawDrugCom2Interaction>();
        public DbSet<Drugs2InteractionRow> InteractionRows => Set<Drugs2InteractionRow>();

        public DrugsCom2Context(DbContextOptions<DrugsCom2Context> options) : base(options)
        {

        }
    }
}
