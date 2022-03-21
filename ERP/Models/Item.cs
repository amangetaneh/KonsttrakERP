using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
