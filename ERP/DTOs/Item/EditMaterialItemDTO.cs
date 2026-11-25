namespace ERP.DTOs.Item
{
    public class EditMaterialItemDTO
    {
        public int ItemId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Spec { get; set; } = string.Empty;

        public string Unit { get; set; } = string.Empty;

        public bool IsTransferable { get; set; }

        public decimal Cost { get; set; }

    }
}
