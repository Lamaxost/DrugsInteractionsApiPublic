using DrugsInteractionsApi.Services.EntityFramework.Entities.Shared;

namespace DrugsInteractionsApi.Services.EntityFramework.Entities.CheckMed
{
    public class CheckMedInteractionRow:DrugsInteractionRow
    {
        public string? Link { get; set; }
    }
}
