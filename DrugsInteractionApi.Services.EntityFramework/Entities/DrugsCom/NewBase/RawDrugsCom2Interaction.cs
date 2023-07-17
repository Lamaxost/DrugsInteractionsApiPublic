using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace DrugsInteractionApi.Services.EntityFramework.Entities.DrugsCom.NewBase
{
    [Index(nameof(Drug1Id))]
    [Index(nameof(Drug2Id))]
    public class RawDrugCom2Interaction
    {
        public int Id { get; set; }

        public int Drug1Id { get; set; }
        public int Drug2Id { get; set; }

        [MaxLength(500)]
        public string Link { get; set; } = default!;
        public string? Html { get; set; }
        public string? HtmlProf { get; set; }
    }
}
