using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
namespace DrugsInteractionsApi.Services.EntityFramework.Entities.Shared
{
    public class DrugsInteractionRow
    {
        public int Id { get; set; }
        [Index(nameof(Drug1Id))]
        public int Drug1Id { get; set; }
        [Index(nameof(Drug2Id))]
        public int Drug2Id { get; set; }
        public string Article { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            if (obj is DrugsInteractionRow row)
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
