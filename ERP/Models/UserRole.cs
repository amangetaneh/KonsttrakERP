using System.ComponentModel.DataAnnotations;

namespace ERP.Models
{
    public class UserRole
    {
        [Key]
        public int RoleId { get; set; }

        public string Role { get; set; } = string.Empty;

        public int CanPurchase { get; set; }

        public int CanRecieve { get; set; }
        
        public int CanIssue { get; set; }
        
        public int CanTransfer { get; set; }
        
        public int CanFixedAsset { get; set; } 
        
        public int CanMaintainance { get; set; }

    }
}
