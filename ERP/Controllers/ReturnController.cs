using ERP.Context;
using ERP.DTOs;
using ERP.Models;
using ERP.Services.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ReturnController : Controller
    {
        private readonly DataContext context;
        private readonly NotificationManager notificationManager;
        private readonly IUserService _userService;

        public UserAccount? UserAccount { get; }

        public Employee Employee { get; }

        public ReturnController(DataContext context, NotificationManager notificationManager, IUserService userService)
        {
            this.context = context;
            this.notificationManager = notificationManager;

            _userService = userService;

            UserAccount = context.UserAccounts
                .Where(u => u.Username == _userService.GetMyName())
                .Include(u => u.UserRole)
                .Include(u => u.Employee)
                .FirstOrDefault();

            Employee = UserAccount.Employee;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Return>> Get(int id)
        {
            var returnResult = await context.Returns.Where(ret => ret.ReturnId == id)
                .Include(ret => ret.ReturnedBy)
                .Include(ret => ret.ReturnEquipmentAssets)
                .ThenInclude(asset => asset.EquipmentAsset)
                .ThenInclude(ea => ea.EquipmentModel)
                .ThenInclude(em => em.Equipment)
                .ThenInclude(equipment => equipment.Item)
                .Include(ret => ret.ReturnEquipmentAssets)
                .ThenInclude(ea => ea.Borrow)
                .Include(ret => ret.ReturnEquipmentAssets)
                .ThenInclude(ea => ea.AssetDamage)
                .Include(ret => ret.Site)
                .FirstOrDefaultAsync();

            if (returnResult == null) return NotFound("Return Not Found.");

            return Ok(returnResult);
        }

        [HttpGet]
        public async Task<ActionResult<List<Borrow>>> Get()
        {

            var returns = await context.Returns
                .Include(ret => ret.Site)
                .ToListAsync();

            return Ok(returns);
        }


        [HttpPost("return")]
        public async Task<ActionResult<int>> ReturnEquipments(ReturnBorrowDTO returnDTO)
        {

            List<BorrowItemEquipmentAsset> equipmentAssets = new();

            if (returnDTO.BorrowAssets == null || returnDTO.BorrowAssets.Count == 0)
                return BadRequest("No Assets Provided to Return.");

            foreach (var requestAsset in returnDTO.BorrowAssets)
            {
                var asset = await context.BorrowItemEquipmentAssets
                    .Where(ea => ea.ItemId == requestAsset.ItemId &&
                        ea.EquipmentModelId == requestAsset.EquipmentModelId &&
                        ea.EquipmentAssetId == requestAsset.EquipmentAssetId &&
                        ea.ReturnId == null)
                    .FirstOrDefaultAsync();

                if (asset == null)
                    return NotFound($"Borrow Item with Id {requestAsset.ItemId},  Model Id {requestAsset.EquipmentModelId}, Asset Id {requestAsset.EquipmentAssetId} Not Found In Borrowed Equiments");

                asset.AssetDamageId = requestAsset.AssetDamageId;
                asset.ReturnRemark = requestAsset.ReturnRemark;

                equipmentAssets.Add(asset);
            }

            Return borrowReturn = new();
            borrowReturn.ReturnedById = Employee.EmployeeId;
            borrowReturn.SiteId = returnDTO.SiteId;

            borrowReturn.ReturnEquipmentAssets = equipmentAssets;

            context.Returns.Add(borrowReturn);
            await context.SaveChangesAsync();

            return Ok(borrowReturn.ReturnId);

        }

        [HttpPost("assets")]
        public async Task<ActionResult<List<EquipmentAsset>>> GetReturnableItems(GetReturnItemsDTO returnItemsDTO)
        {
            List<BorrowItemEquipmentAsset> borrowAssets = await context.BorrowItemEquipmentAssets
                    .Where(ea => ea.Borrow.RequestedById == returnItemsDTO.RequestedById
                            && (returnItemsDTO.ItemId == null || ea.ItemId == returnItemsDTO.ItemId)
                            && ea.ReturnId == null)
                    .Include(ea => ea.EquipmentAsset.EquipmentModel.Equipment)
                    .ToListAsync();

            List<EquipmentAsset> assets = new();

            foreach (var borrowAsset in borrowAssets)
                assets.Add(borrowAsset.EquipmentAsset);
            
            return Ok(assets);

        }

        [HttpGet("damage")]
        public async Task<ActionResult<List<AssetDamage>>> GetDamageTypes()
        {
            var damages = await context.AssetDamages.ToListAsync();

            return Ok(damages);
        }

    }
}
