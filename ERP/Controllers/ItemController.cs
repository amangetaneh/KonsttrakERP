using ERP.Context;
using ERP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : Controller
    {
        private readonly DataContext context;

        public ItemController(DataContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Item>>> GetAll()
        {
            var items = await context.Items.Include(i => i.Material).Include(i => i.Equipment).ToListAsync();

            return Ok(items);
        }

        [HttpGet("{ItemId}")]
        public async Task<ActionResult<Item>> GetOne(int ItemId)
        {
            var item = await context.Items
                .Where(item => item.ItemId == ItemId)
                .Include(i => i.Material)
                .Include(i => i.Equipment)
                .ThenInclude(e => e.EquipmentModels)
                .FirstOrDefaultAsync();

            if (item == null) return NotFound("Item Not Found");

            return Ok(item);

        }

        [HttpGet("equipmentmodel/{ModelId}")]
        public async Task<ActionResult<EquipmentModel>> GetModel(int ModelId)
        {
            var model = await context.EquipmentModels
                .Where(model => model.EquipmentModelId == ModelId)
                .Include(model => model.EquipmentAssets)
                .FirstOrDefaultAsync();

            if (model == null) return NotFound("Equipment Model Not Found");

            return Ok(model);

        }

        [HttpGet("material")]
        public async Task<ActionResult<List<Item>>> GetMaterial()
        {
            var items = await context.Items.Where(item => item.Type == ITEMTYPE.MATERIAL)
                .Include(i => i.Material).ToListAsync();

            return Ok(items);
        }

        [HttpGet("equipment")]
        public async Task<ActionResult<List<Item>>> GetEquipment()
        {
            var items = await context.Items.Where(item => item.Type == ITEMTYPE.EQUIPMENT)
                .Include(i => i.Equipment).ToListAsync();

            return Ok(items);
        }
    }
}
