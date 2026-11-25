namespace ERP.DTOs.Item
{
    public class CreateMaterialItemDTO
    {
        public string Name { get; set; } = string.Empty;

        public string Spec { get; set; } = string.Empty;

        public string Unit { get; set; } = string.Empty;

        public bool IsTransferable { get; set; }

        public decimal Cost { get; set; }

    }
}
