using ERP.Context;
using ERP.DTOs;
using ERP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceController : Controller
    {
        private readonly DataContext context;
        private readonly NotificationManager notificationManager;

        public MaintenanceController(DataContext context, NotificationManager notificationManager)
        {
            this.context = context;
            this.notificationManager = notificationManager;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Maintenance>> Get(int id)
        {
            var maintenance = context.Maintenances.Where(maintenance => maintenance.MaintenanceId == id)
                .Include(maintenance => maintenance.Site)
                .Include(maintenance => maintenance.RequestedBy)
                .Include(maintenance => maintenance.ApprovedBy)
                .Include(maintenance => maintenance.FixedBy)
                .Include(maintenance => maintenance.Item)
                .ThenInclude(item => item.Equipment)
                .Include(maintenance => maintenance.EquipmentAsset)
                .FirstOrDefault();

            if (maintenance == null) return NotFound("Maintenance Request Not Found.");

            return Ok(maintenance);
        }

        [HttpGet]
        public async Task<ActionResult<List<Maintenance>>> Get()
        {

            var maintenance = await context.Maintenances
                .Include(maintenance => maintenance.Site)
                .ToListAsync();

            return Ok(maintenance);
        }

        [HttpPost("request")]
        public async Task<ActionResult<List<Maintenance>>> Post(CreateMaintenanceDTO maintenanceDTO)
        {
            Maintenance maintenance = new();
            maintenance.RequestedById = maintenanceDTO.RequestedById;
            maintenance.SiteId = maintenanceDTO.SiteId;
            maintenance.Reason = maintenanceDTO.Reason;

            var itemTemp = context.Items.Where(item => item.ItemId == maintenanceDTO.ItemId).FirstOrDefault();

            if (itemTemp == null) return NotFound($"Maintenance Item with Id {maintenanceDTO.ItemId} Not Found");

            if (itemTemp.Type != ITEMTYPE.EQUIPMENT) 
                return BadRequest($"Maintenance Item with Id {maintenanceDTO.ItemId} Is Not Type of Equipment");

            var maintenanceEquipmentModel = await context.EquipmentModels
                .Where(em => em.EquipmentModelId == maintenanceDTO.EquipmentModelId && em.ItemId == maintenanceDTO.ItemId)
                .FirstOrDefaultAsync();

            if (maintenanceEquipmentModel == null) 
                return NotFound($"Maitenance Item with Id {maintenanceDTO.ItemId} and Model Id {maintenanceDTO.EquipmentModelId} Not Found");

            if (maintenanceDTO.EquipmentAssetId != null)
            {
                var maintenanceEquipmentAsset = await context.EquipmentAssets
                    .Where(ea => ea.EquipmentAssetId == maintenanceDTO.EquipmentAssetId && ea.EquipmentModelId == maintenanceDTO.EquipmentModelId)
                    .FirstOrDefaultAsync();

                if (maintenanceEquipmentAsset == null)
                    return NotFound($"Maitenance Item with Model Id {maintenanceDTO.EquipmentModelId} and Asset Id {maintenanceDTO.EquipmentAssetId} Not Found");

                maintenance.EquipmentAssetId = maintenanceDTO.EquipmentAssetId;
            }

            maintenance.Cost = maintenanceEquipmentModel.Cost;

            maintenance.ItemId = maintenanceDTO.ItemId;
            maintenance.EquipmentModelId = maintenanceDTO.EquipmentModelId;

            context.Maintenances.Add(maintenance);
            await context.SaveChangesAsync();

            await notificationManager.Add(type: NOTIFICATIONTYPE.MAINTAINANCE,
                status: maintenance.Status,
                actionId: maintenance.MaintenanceId,
                siteId: maintenance.SiteId,
                employeeId: maintenance.RequestedById);

            return Ok(maintenance.MaintenanceId);
        }

        [HttpPost("approve")]
        public async Task<ActionResult<Maintenance>> Approve(ApproveMaintenanceDTO approveDTO)
        {
            var maintenance = await context.Maintenances
                .Where(m => m.MaintenanceId == approveDTO.MaintenanceId)
                .FirstOrDefaultAsync();
            if (maintenance == null) return NotFound("Maintenance Request Not Found.");

            maintenance.ApproveDate = DateTime.Now;
            maintenance.ApprovedById = approveDTO.ApprovedById;
            maintenance.ApproveRemark = approveDTO.ApproveRemark;

            maintenance.Status = MAINTENANCESTATUS.APPROVED;

            await context.SaveChangesAsync();

            await notificationManager.Add(type: NOTIFICATIONTYPE.MAINTAINANCE,
                status: maintenance.Status,
                actionId: maintenance.MaintenanceId,
                siteId: maintenance.SiteId,
                employeeId: maintenance.RequestedById);

            return Ok(maintenance);
        }

        [HttpPost("decline")]
        public async Task<ActionResult<Maintenance>> Decline(ApproveMaintenanceDTO declineDTO)
        {
            var maintenance = await context.Maintenances
                .Where(m => m.MaintenanceId == declineDTO.MaintenanceId)
                .FirstOrDefaultAsync();
            if (maintenance == null) return NotFound("Maintenance Request Not Found.");

            maintenance.ApproveDate = DateTime.Now;
            maintenance.ApprovedById = declineDTO.ApprovedById;
            maintenance.ApproveRemark = declineDTO.ApproveRemark;

            maintenance.Status = MAINTENANCESTATUS.DECLINED;

            await context.SaveChangesAsync();

            await notificationManager.Add(type: NOTIFICATIONTYPE.MAINTAINANCE,
                status: maintenance.Status,
                actionId: maintenance.MaintenanceId,
                siteId: maintenance.SiteId,
                employeeId: maintenance.RequestedById);

            return Ok(maintenance);
        }

        [HttpPost("fix")]
        public async Task<ActionResult<Maintenance>> fix(FixMaintenanceDTO fixDTO)
        {
            var maintenance = await context.Maintenances
                .Where(m => m.MaintenanceId == fixDTO.MaintenanceId)
                .FirstOrDefaultAsync();
            if (maintenance == null) return NotFound("Maintenance Request Not Found.");


            maintenance.FixDate = DateTime.Now;
            maintenance.FixedById = fixDTO.FixedById;
            maintenance.FixRemark = fixDTO.FixRemark;

            maintenance.Status = MAINTENANCESTATUS.FIXED;

            await context.SaveChangesAsync();

            await notificationManager.Add(type: NOTIFICATIONTYPE.MAINTAINANCE,
                status: maintenance.Status,
                actionId: maintenance.MaintenanceId,
                siteId: maintenance.SiteId,
                employeeId: maintenance.RequestedById);

            return Ok(maintenance);
        }

    }
}
