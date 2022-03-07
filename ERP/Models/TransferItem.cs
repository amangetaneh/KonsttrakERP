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

        public int? QtyTransfered { get; set; }

        public int? QtyReceived { get; set; }   

        public Double Cost { get; set; }

        public string RequestRemark { get; set; } = string.Empty;
        
        public string SendRemark { get; set; } = string.Empty;
       
        public string ReceiveRemark { get; set; } = string.Empty;
       
        [JsonIgnore]

        [ForeignKey("TransferId")]
        public Transfer Transfer { get; set; }
        
        [ForeignKey("ItemId")]
        public Item Item { get; set; }


    }
}
