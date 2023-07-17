using DrugsInteractionsApi.Services.EntityFramework.Entities.Shared;
namespace DrugsInteractionApi.Services.EntityFramework.Entities.DrugsCom.OldBase
{
    public class DrugsComDrug : Drug
    {
        public int[]? GenericIds { get; set; } = new int[0];
    }
}
