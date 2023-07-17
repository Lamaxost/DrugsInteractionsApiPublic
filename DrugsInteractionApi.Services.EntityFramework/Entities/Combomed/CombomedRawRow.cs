namespace DrugsInteractionsApi.Services.EntityFramework.Entities.ComboMed
{
    public class CombomedRawInteraction
    {
        public string Html { get; set; } = default!;
        public string Link { get; set; } = default!;

        public bool Parsed { get; set; }
        public int Id { get; set; }
    }
}
