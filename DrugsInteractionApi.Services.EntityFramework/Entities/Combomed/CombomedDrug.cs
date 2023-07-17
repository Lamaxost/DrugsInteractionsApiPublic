using DrugsInteractionsApi.Services.EntityFramework.Entities.Shared;
namespace DrugsInteractionsApi.Services.EntityFramework.Entities.ComboMed
{
    public class CombomedDrug:Drug
    {
        public int[] GenericIds { get; set; } = new int[0];
    }
}
