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
        
        public ConsumableItem ConsumableItem { get; set; }
        
        public NonConsumableItem NonConsumableItem { get; set; }


    }
}
