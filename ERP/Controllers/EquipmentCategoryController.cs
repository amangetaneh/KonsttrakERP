using ERP.Context;
using ERP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentCategoryController : Controller
    {
        private readonly DataContext context;

        public EquipmentCategoryController(DataContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<EquipmentCategory>>> GetAll()
        {
            var categories = await context.EquipmentCategories.ToListAsync();

            return Ok(categories);
        }

        [HttpGet("{EquipmentCategoryId}")]
        public async Task<ActionResult<List<EquipmentCategory>>> GetOne(int EquipmentCategoryId)
        {
            var category = await context.EquipmentCategories
                .Where(ec => ec.EquipmentCategoryId == EquipmentCategoryId)
                .Include(ec => ec.Equipments)
                .ThenInclude(equipment => equipment.Item)
                .FirstOrDefaultAsync();

            if (category == null) return BadRequest("Equipment Category Not Found");

            return Ok(category);
        }
    }
}
