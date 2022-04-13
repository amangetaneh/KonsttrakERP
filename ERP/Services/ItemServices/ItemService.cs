using ERP.Context;
using ERP.DTOs.Item;
using ERP.Models;
using ERP.Services.User;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.ItemServices
{
    public class ItemService : IItemService
    {
        private readonly DataContext _context;
        private readonly IUserService _userService;

        public ItemService(DataContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<List<Item>> GetAll()
        {
            var items = await _context.Items
                .Include(i => i.Material)
                .Include(i => i.Equipment)
                .ToListAsync();

            return items;
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

        public async Task<int> GetItemQty(GetQtyDTO qtyDTO)
        {

            int qty = 0;

            var siteId = qtyDTO.SiteId != -1 ? qtyDTO.SiteId : _userService.Employee.EmployeeSiteId;

            if (siteId == null) return qty;

            if (qtyDTO.EquipmentModelId != -1)
            {
                var temp  = await _context.EquipmentSiteQties
                       .Where(esq => esq.EquipmentModelId == qtyDTO.EquipmentModelId && esq.SiteId == siteId)
                       .FirstOrDefaultAsync();
                
                if (temp != null) qty = temp.Qty;

            }


            if (qtyDTO.ItemId != -1)
            {
                var temp = await _context.MaterialSiteQties
                       .Where(esq => esq.ItemId == qtyDTO.ItemId && esq.SiteId == siteId)
                       .FirstOrDefaultAsync();

                if (temp != null) qty = temp.Qty;

            }

            return qty;
        }
    }
}
