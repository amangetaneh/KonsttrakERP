using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ERP.Models
{
    public class PurchaseItemSite
    {
        [Key]
        public int PurchaseId { get; set; }

        [Key]
        public int ItemId { get; set; }

        [Key]
        public int RequestSiteId { get; set; }

        [JsonIgnore]
        [ForeignKey("PurchaseId")]
        public Purchase Purchase { get; set; }

        [ForeignKey("ItemId")]
        public Item Item { get; set; }

        [ForeignKey("RequestingSiteId")]
        public Site Site { get; set; }
    }
}
