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
        public async Task<ActionResult<List<Item>>> Get()
        {
            var stores = await context.Items.Include(i => i.Material).Include(i => i.Equipment).ToListAsync();

            return Ok(stores);
        }

        [HttpGet("material")]
        public async Task<ActionResult<List<Item>>> GetMaterial()
        {
            var stores = await context.Items.Where(item => item.Type == ITEMTYPE.MATERIAL)
                .Include(i => i.Material).ToListAsync();

            return Ok(stores);
        }

        [HttpGet("equipment")]
        public async Task<ActionResult<List<Item>>> GetEquipment()
        {
            var stores = await context.Items.Where(item=> item.Type == ITEMTYPE.EQUIPMENT)
                .Include(i => i.Equipment).ToListAsync();

            return Ok(stores);
        }
    }
}
