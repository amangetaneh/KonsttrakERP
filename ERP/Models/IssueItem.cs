using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ERP.Models
{
    public class IssueItem
    {
        [Key]
        public int IssueId { get; set; }

        [Key]
        public int ItemId { get; set; }

        public int QtyRequested { get; set; }

        public int QtyIssued { get; set; }

        public Double Cost { get; set; }

        public string Remark { get; set; }
        [JsonIgnore]
        [ForeignKey("IssueId")]
        public Issue Issue { get; set; }
        [JsonIgnore]
        [ForeignKey("ItemId")]
        public Item Item { get; set; }

    }
}
