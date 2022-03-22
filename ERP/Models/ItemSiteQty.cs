using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Models
{
    public class ItemSiteQty
    {
        [Key]
        public int ItemId { get; set; }
        [Key]
        public int SiteId { get; set; }

        public int Qty { get; set; }

        [ForeignKey("ItemId")]
        public Item Item { get; set; }

        [ForeignKey("SiteId")]
        public Site Site { get; set; }

    }
}
