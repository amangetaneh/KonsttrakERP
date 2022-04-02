using ERP.Context;
using ERP.Models;
using ERP.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP.DTOs.Item;
using ERP.Services.ItemServices;
using Microsoft.AspNetCore.Authorization;

namespace ERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Employee")]
    public class ItemController : Controller
    {
        private readonly DataContext context;
        private readonly IItemService _itemService;

        public ItemController(DataContext context, IItemService itemService)
        {
            this.context = context;
            _itemService = itemService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Item>>> GetAll()
        {
            var items = await  _itemService.GetAll();

            return Ok(items);
        }

        [HttpGet("{ItemId}")]
        public async Task<ActionResult<Item>> GetOne(int ItemId)
        {
           
            try
            {
                var item = await _itemService.GetById(ItemId);
                return Ok(item);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

        }

        [HttpGet("equipmentmodel/{ModelId}")]
        public async Task<ActionResult<EquipmentModel>> GetModel(int ModelId)
        {

            try
            {
                var model = await _itemService.GetModel(ModelId);
                return Ok(model);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

        }

        [HttpGet("material")]
        public async Task<ActionResult<List<Item>>> GetMaterial()
        {
            var items = await _itemService.GetMaterials();

            return Ok(items);
        }

        [HttpGet("equipment")]
        public async Task<ActionResult<List<Item>>> GetEquipment()
        {
            var items = await _itemService.GetEquipments();

            return Ok(items);
        }

        [HttpPost("material/add")]
        public async Task<ActionResult<List<Item>>> CreateMaterialItem(CreateMaterialItemDTO materialItemDTO)
        {
            Item item = new();
            item.Type = ITEMTYPE.MATERIAL;
            item.Name = materialItemDTO.Name;

            context.Items.Add(item);

            await context.SaveChangesAsync();

            Material material = new();
            material.ItemId = item.ItemId;
            material.Spec = materialItemDTO.Spec;
            material.Unit = materialItemDTO.Unit;
            material.Cost = materialItemDTO.Cost;

            context.Materials.Add(material);

            await context.SaveChangesAsync();

            return Ok(item);
        }

        [HttpPost("equipment/add")]
        public async Task<ActionResult<List<Item>>> CreateEquipmentItem(CreateEquipmentItemDTO equipmentItemDTO)
        {
            Item item = new();
            item.Type = ITEMTYPE.EQUIPMENT;
            item.Name = equipmentItemDTO.Name;

            context.Items.Add(item);

            await context.SaveChangesAsync();

            Equipment equipment = new();
            equipment.ItemId = item.ItemId;
            equipment.Unit = equipmentItemDTO.Unit;
            equipment.Description = equipmentItemDTO.Description;
            equipment.EquipmentCategoryId = equipmentItemDTO.EquipmentCategoryId;

            context.Equipments.Add(equipment);

            await context.SaveChangesAsync();

            return Ok(item);
        }

    }
}
