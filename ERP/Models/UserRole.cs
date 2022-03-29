using System.ComponentModel.DataAnnotations;

namespace ERP.Models
{
    public class UserRole
    {
        [Key]
        public int RoleId { get; set; }

        public string Role { get; set; } = string.Empty;

        public int CanRequestPurchase { get; set; } = 0;

        public int CanApprovePurchase { get; set; } = 0;

        public int CanCheckPurchase { get; set; } = 0;
        
        public int CanViewPurchase { get; set; } = 0;

        public int CanRequestBuy { get; set; } = 0;

        public int CanApproveBuy { get; set; } = 0;

        public int CanCheckBuy { get; set; } = 0;

        public int CanViewBuy { get; set; } = 0;

        public int CanRecieve { get; set; } = 0;

        public int CanCheckRecieve { get; set; } = 0;
        
        public int CanViewRecieve { get; set; } = 0;

        public int CanRequestIssue { get; set; } = 0;

        public int CanApproveIssue { get; set; } = 0;
        
        public int CanHandIssue { get; set; } = 0; 
        
        public int CanViewIssue { get; set; } = 0; 
        
        public int CanRequestBorrow { get; set; } = 0;

        public int CanApproveBorrow { get; set; } = 0;
        
        public int CanHandBorrow { get; set; } = 0;
        
        public int CanViewBorrow { get; set; } = 0;

        public int CanReturnBorrow { get; set; } = 0;

        public int CanRequestEquipmentTransfer { get; set; } = 0;

        public int CanRequestMaterialTransfer { get; set; } = 0;
        
        public int CanApproveEquipmentTransfer { get; set; } = 0;

        public int CanApproveMaterialTransfer { get; set; } = 0;
        
        public int CanHandEquipmentTransfer { get; set; } = 0;

        public int CanHandMaterialTransfer { get; set; } = 0;
        
        public int CanViewEquipmentTransfer { get; set; } = 0;

        public int CanViewMaterialTransfer { get; set; } = 0;

        public int CanRequestMaintainance { get; set; } = 0;

        public int CanApproveMaintainance { get; set; } = 0;
       
        public int CanFixMaintainance { get; set; } = 0;
       
        public int CanViewMaintainance { get; set; } = 0;

    }
}
