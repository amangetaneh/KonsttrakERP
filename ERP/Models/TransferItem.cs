using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ERP.Models
{
    public class TransferItem
    {
        [Key]
        public int TransferId { get; set; }

        [Key]
        public int ItemId { get; set; }

        public int QtyRequested { get; set; }

        public int QtyTransfered { get; set; }

        public Double Cost { get; set; }

        public string Remark { get; set; }
        [JsonIgnore]

        [ForeignKey("TransferId")]
        public Transfer Transfer { get; set; }
        [JsonIgnore]
        [ForeignKey("ItemId")]
        public Item Item { get; set; }


    }
}
