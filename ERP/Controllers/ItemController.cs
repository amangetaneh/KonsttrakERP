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
            var stores = await context.Items.Include(i => i.ConsumableItem).Include(i => i.NonConsumableItem).ToListAsync();

            return Ok(stores);
        }

        [HttpGet("consumable")]
        public async Task<ActionResult<List<Item>>> GetConsumable()
        {
            var stores = await context.Items.Where(item => item.Type == 1)
                .Include(i => i.ConsumableItem).ToListAsync();

            return Ok(stores);
        }

        [HttpGet("nonconsumable")]
        public async Task<ActionResult<List<Item>>> GetNonConsumable()
        {
            var stores = await context.Items.Where(item=> item.Type == 2)
                .Include(i => i.NonConsumableItem).ToListAsync();

            return Ok(stores);
        }
    }
}
