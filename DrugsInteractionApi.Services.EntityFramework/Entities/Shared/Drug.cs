using DrugsInteractionApi.Services.Entities.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DrugsInteractionsApi.Services.EntityFramework.Entities.Shared
{
    public class Drug
    {
        [Key]
        public int Id { get; set; }
        [Index(nameof(NameRu))]
        public string? NameRu { get; set; }
        [Index(nameof(NameEn))]
        public string? NameEn { get; set; }
        [Index(nameof(NameUk))]
        public string? NameUk { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is DrugReponse drug)
            {
                return NameRu == drug.NameRu && NameUk == drug.NameUk && NameEn == drug.NameEn;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return (NameRu + NameUk + NameEn).GetHashCode();
        }
    }
}
