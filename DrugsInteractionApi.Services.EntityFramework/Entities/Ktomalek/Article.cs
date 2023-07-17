using System.ComponentModel.DataAnnotations;

namespace DrugsInteractionApi.Services.EntityFramework.Entities.Ktomalek
{
    public class KtomalekArticle
    {
        [Key]
        public int Id { get; set; }
        public string Article { get; set; } = string.Empty;
    }

}