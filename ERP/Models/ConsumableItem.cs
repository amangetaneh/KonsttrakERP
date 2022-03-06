using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Models
{
    
    public class ConsumableItem
    {
        [Key]
        public int ItemId { get; set; }

        [ForeignKey("ItemId")]
        public Item Item { get; set; }

        public string Spec { get; set; } = string.Empty;

        public string Unit { get; set; } = string.Empty;

    }
}
