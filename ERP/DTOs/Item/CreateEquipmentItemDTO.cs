namespace ERP.DTOs.Item
{
    public class CreateEquipmentItemDTO
    {
        public string Name { get; set; } = string.Empty;

        public string Unit { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int EquipmentCategoryId { get; set; }

    }
}
