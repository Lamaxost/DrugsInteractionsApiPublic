using DrugsInteractionsApi.Services.EntityFramework.Entities.Shared;
namespace DrugsInteractionApi.Services.EntityFramework.Entities.DrugsCom.NewBase
{
    public class Drugs2InteractionRow : DrugsInteractionRow
    {
        public string? Link { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is Drugs2InteractionRow row)
            {
                return row.Id == Id;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
