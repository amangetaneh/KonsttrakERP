using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Models
{
    public class ReceiveItem
    {
        [Key]
        public int ReceiveId { get; set; }

        [Key]
        public int ItemId { get; set; }

        public int Qty { get; set; }

        public Double Cost { get; set; }

        public Double TotalCost => (Qty * Cost);

        public string Remark { get; set; }

        [ForeignKey("ReceiveId")]
        public Receive Receive { get; set; }

        [ForeignKey("ItemId")]
        public Item Item { get; set; }

    }
}
