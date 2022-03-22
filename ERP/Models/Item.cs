using System.ComponentModel.DataAnnotations;

namespace ERP.Models
{
    public class Item
    {
        [Key]
        public int ItemId { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Type { get; set; }

        public Material Material { get; set; }

        public Equipment Equipment { get; set; }


    }
}
