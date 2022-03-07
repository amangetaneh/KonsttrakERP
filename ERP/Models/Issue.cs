using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ERP.Models
{
    public class Issue
    {
        [Key]
        public int Id { get; set; }

        public int Status { get; set; } = 0;

        [Column(TypeName = "Date")]
        public DateTime Date { get; set; } = DateTime.Today;
        [JsonIgnore]
        private Site Site { get; set; }

        private int SiteId { get; set; }

        [JsonIgnore]
        public Employee RequestedBy { get; set; }

        public int RequestedById { get; set; }

        [JsonIgnore]
        public Employee HandedBy { get; set; }

        public int? HandedById { get; set; }
       [JsonIgnore]
        public Employee ApprovedBy { get; set; }

        public int? ApprovedById { get; set; }

        public ICollection<IssueItem> IssueItems { get; set; }
    }
}