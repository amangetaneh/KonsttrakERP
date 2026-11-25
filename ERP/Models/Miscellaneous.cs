using System.ComponentModel.DataAnnotations;

namespace ERP.Models
{
    public class Miscellaneous
    {
        [Key]
        public string Key { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;
    }
}
