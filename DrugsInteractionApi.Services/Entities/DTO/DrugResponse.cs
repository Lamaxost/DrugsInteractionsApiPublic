using System.IO.Pipes;

namespace DrugsInteractionApi.Services.Entities.DTO
{
    public class DrugReponse
    {
        public int Id { get; set; }
        public string NameRu { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string NameUk { get; set; } = string.Empty;

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
            return (NameRu + NameUk + NameEn).GetHashCode(StringComparison.Ordinal);
        }
    }
}
