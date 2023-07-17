using System.ComponentModel.DataAnnotations;
using DrugsInteractionsApi.Services.EntityFramework.Entities.Shared;

namespace DrugsInteractionApi.Services.EntityFramework.Entities.DrugsCom.NewBase
{
    public class DrugsCom2Drug : Drug
    {
        [MaxLength(500)]
        public string Url { get; set; } = default!;
        public int? SiteId { get; set; }
        public List<int>? GenericIds { get; set; }
        public List<int>? BrandNames { get; set; }
        public bool IsGeneric { get; set; }
    }
}
