using DrugsInteractionsApi.Services.EntityFramework.Entities.Shared;
using Microsoft.EntityFrameworkCore;

namespace DrugsInteractionsApi.Services.EntityFramework.Entities.ComboMed
{
    public class CombomedNewContext : CombomedOldContext
    {
        public CombomedNewContext(DbContextOptions<CombomedOldContext> options) : base(options)
        {

        }
    }
}
