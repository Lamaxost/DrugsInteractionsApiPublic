using DrugsInteractionsApi.Services.EntityFramework.Entities.Shared;

namespace DrugsInteractionsApi.Services.EntityFramework.Entities.ComboMed
{
    public class CombomedInteractionRow : DrugsInteractionRow
    {
        public string? ActingSubstance1 { get; set; }
        public string? ActingSubstance2 { get; set; }
        public string? Drug1Group { get; set; }
        public string? Drug2Group { get; set; }
        public string? Article2 { get; set; } = string.Empty;
    }
}
