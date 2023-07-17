namespace DrugsInteractionApi.Services.EntityFramework.Entities.NotFound
{
    public class NotFoundMessage
    {
        public int Id { get; set; }
        public string Drug1Name { get; set; } = default!;
        public string Drug2Name { get; set; } = default!;
        public bool Drug1Found { get; set; }
        public bool Drug2Found { get; set; }
        public bool InteractionFound { get; set; }
    }
}
