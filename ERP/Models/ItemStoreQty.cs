using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Models
{
    public class ItemStoreQty
    {
        [Key]
        public int ItemId { get; set; }
        [Key]
        public int StoreId { get; set; }
    
        public int Qty { get; set; }

        [ForeignKey("ItemId")]
        public Item Item { get; set; }

        [ForeignKey("StoreId")]
        public Store Store { get; set; }

    }
}
