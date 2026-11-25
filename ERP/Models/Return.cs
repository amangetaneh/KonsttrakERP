using System.ComponentModel.DataAnnotations;

namespace ERP.Models
{
    public class Return

    {
        [Key]
        public int ReturnId { get; set; }

        public int ReturnedById { get; set; }

        public Employee ReturnedBy { get; set; } = null!;

        public int SiteId { get; set; }

        public Site Site { get; set; } = null!;

        public DateTime ReturnDate { get; set; } = DateTime.Now;

        public ICollection<BorrowItemEquipmentAsset> ReturnEquipmentAssets { get; set; } = [];
    }
}
