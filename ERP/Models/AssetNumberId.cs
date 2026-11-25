using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Models
{
    public class AssetNumberId
    {

        [Key]
        public int Id { get; set; }

        public int ItemId { get; set; }

        [ForeignKey("ItemId")]
        public Item Item { get; set; } = null!;

        public string Prefix { get; set; } = string.Empty;

        public int LastId { get; set; } = 0;


    }
}
