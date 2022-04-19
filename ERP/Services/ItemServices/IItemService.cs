using ERP.DTOs.Item;
using ERP.Models;

namespace ERP.Services.ItemServices
{
    public interface IItemService
    {
        Task<List<Item>> GetAll();
        
        Task<Item> GetById(int id);
        
        Task<List<Item>> GetEquipments();
        Task<int> GetItemQty(GetQtyDTO qtyDTO);
        
        Task<List<Item>> GetMaterials();
        
        Task<List<EquipmentCategory>> GetEquipmentCategories();

        Task<EquipmentModel> GetModel(int modelId);
        Task<List<Item>> GetTransferableMaterials();
    }
}
