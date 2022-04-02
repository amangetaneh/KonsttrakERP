using ERP.Models;

namespace ERP.Services.ItemServices
{
    public interface IItemService
    {
        Task<List<Item>> GetAll();
        Task<Item> GetById(int id);
        Task<List<Item>> GetEquipments();
        Task<List<Item>> GetMaterials();
        Task<EquipmentModel> GetModel(int modelId);
    }
}
