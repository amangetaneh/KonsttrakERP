namespace ERP.DTOs.Item
{
    public class CreateEquipmentModelDTO
    {
        public int ItemId { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Cost { get; set; }

    }
}
