using DrugsInteractionsApi.Services.EntityFramework.Entities.Shared;

namespace DrugsInteractionApi.Services.EntityFramework.Entities.Ktomalek
{
    public class KtomalekDrug : Drug
    {
        public string NamePl { get; set; } = default!;
        public bool? DoesInteract { get; set; }
        public string? Link { get; set; }
        public bool Parsed { get; set; } = false;
    }
}
