using ERP.Context;
using ERP.Models;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.ItemServices
{
    public class ItemService : IItemService
    {
        private readonly DataContext _context;

        public ItemService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Item>> GetAll()
        {
            var items = await _context.Items
                .Include(i => i.Material)
                .Include(i => i.Equipment)
                .ToListAsync();

            return items;
        }

        public async Task<List<EquipmentCategory>> GetEquipmentCategories()
        {
            var categories = await _context.EquipmentCategories.ToListAsync();

            return categories;
        }
        
        public async Task<List<Item>> GetMaterials()
        {
            var items = await _context.Items.Where(item => item.Type == ITEMTYPE.MATERIAL)
                .Include(i => i.Material).ToListAsync();

            return items;
        }

        public async Task<List<Item>> GetEquipments()
        {
            var items = await _context.Items.Where(item => item.Type == ITEMTYPE.EQUIPMENT)
                .Include(i => i.Equipment).ToListAsync();

            return items;
        }

        public async Task<Item> GetById(int id)
        {
            var item = await _context.Items
               .Where(item => item.ItemId == id)
               .Include(i => i.Material)
               .Include(i => i.Equipment)
               .ThenInclude(e => e.EquipmentModels)
               .FirstOrDefaultAsync();

            if (item == null) throw new KeyNotFoundException("Item Not Found");

            return item;
        }

        public async Task<EquipmentModel> GetModel(int modelId)
        {
            var model = await _context.EquipmentModels
                .Where(model => model.EquipmentModelId == modelId)
                .Include(model => model.EquipmentAssets)
                .FirstOrDefaultAsync();

            if (model == null) throw new KeyNotFoundException("Equipment Model Not Found");

            return model;
        }
    }
}
