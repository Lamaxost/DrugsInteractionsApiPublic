using System.ComponentModel.DataAnnotations.Schema;
using DrugsInteractionsApi.Services.EntityFramework.Entities.Shared;

namespace DrugsInteractionApi.Services.EntityFramework.Entities.Ktomalek
{
    public class KtomalekInteractionRow : DrugsInteractionRow
    {
        public string? Character { get; set; }

        public int? ArticleId { get; set; }

        [ForeignKey(nameof(ArticleId))]
        public new KtomalekArticle? Article { get; set;}
    }
}
