using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ERP.Models
{
    public class ReceiveItem
    {
        [Key]
        public int ReceiveId { get; set; }

        [Key]
        public int ItemId { get; set; }

        public int QtyReceived { get; set; }

        public string ReceiveRemark { get; set; }

        [JsonIgnore]
        [ForeignKey("ReceiveId")]
        public Receive Receive { get; set; }

        [ForeignKey("ItemId")]
        public Item Item { get; set; }
    }
}
