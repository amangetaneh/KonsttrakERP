using ERP.Context;
using ERP.DTOs;
using ERP.Models;
using ERP.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransferItem = ERP.Models.TransferItem;

namespace ERP.Controllers
{

    [Route("/api/[controller]")]
    [ApiController]
    //comment out the line below to disable the authorization
    //[Authorize(Roles = "Employee")]
    public class TransferController : Controller
    {
        private readonly DataContext context;
        private readonly NotificationManager notificationManager;

        private readonly IUserService _userService;

        public UserAccount? UserAccount { get; }

        public TransferController(DataContext context, IUserService userService, NotificationManager notificationManager)
        {
            this.context = context;
            this.notificationManager = notificationManager;

            _userService = userService;

            UserAccount = context.UserAccounts
                .Where(u => u.Username == _userService.GetMyName())
                .Include(u => u.UserRole)
                .Include(u => u.Employee)
                .FirstOrDefault();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Transfer>> Get(int id)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanViewTransfer != 1) return Forbid();

            var transfer = context.Transfers.Where(transfer => transfer.TransferId == id)
                .Include(transfer => transfer.SendSite)
                .Include(transfer => transfer.ReceiveSite)
                .Include(transfer => transfer.RequestedBy)
                .Include(transfer => transfer.SentBy)
                .Include(transfer => transfer.ReceivedBy)
                .Include(transfer => transfer.ApprovedBy)
                .Include(transfer => transfer.TransferItems)
                .ThenInclude(transferItem => transferItem.Item)
                .ThenInclude(item => item.Material)
                .Include(transfer => transfer.TransferItems)
                .ThenInclude(transferItem => transferItem.Item.Equipment)
                .Include(transfer => transfer.TransferItems)
                .ThenInclude(transferItem => transferItem.TransferEquipmentAssets)
                .ThenInclude(transferEquipmentAsset => transferEquipmentAsset.EquipmentAsset)
                .FirstOrDefault();

            if (transfer == null) return NotFound("Transfer Not Found.");

            return Ok(transfer);
        }

        [HttpGet]
        public async Task<ActionResult<List<Transfer>>> Get()
        {

            var transfers = await context.Transfers
                .Include(transfer => transfer.SendSite)
                .Include(transfer => transfer.ReceiveSite)
                .ToListAsync();

            return Ok(transfers);
        }

        [HttpPost]
        public async Task<ActionResult<List<Transfer>>> GetByParams(GetTransfersDTO getTransfersDTO)
        {

            var transfers = await context.Transfers.
                Where()
                .Include(transfer => transfer.SendSite)
                .Include(transfer => transfer.ReceiveSite)
                .ToListAsync();

            return Ok(transfers);
        }

        [HttpPost("request/equipment")]
        public async Task<ActionResult<List<Transfer>>> RequestEquipment(CreateEquipmentTransferDTO transferDTO)
        {
            Transfer transfer = new();
            transfer.SendSiteId = transferDTO.SendSiteId;
            transfer.ReceiveSiteId = transferDTO.ReceiveSiteId;
            transfer.RequestedById = transferDTO.RequestedById;

            ICollection<TransferItem> transferItems = new List<TransferItem>();

            foreach (var requestItem in transferDTO.TransferItems)
            {
                TransferItem transferItem = new();
                transferItem.ItemId = requestItem.ItemId;
                transferItem.QtyRequested = requestItem.QtyRequested;
                transferItem.RequestRemark = requestItem.RequestRemark;

                var itemTemp = await context.Items.Where(item => item.ItemId == requestItem.ItemId)
                    .Include(i => i.Equipment)
                    .Include(i => i.Material)
                    .FirstOrDefaultAsync();

                if (itemTemp == null) return NotFound($"Transfer Item with Id {requestItem.ItemId} Not Found");

                if (itemTemp.Type != ITEMTYPE.EQUIPMENT) return BadRequest($"Transfer Item with Id {requestItem.ItemId} Is Not Type of Equipment");

                transferItem.EquipmentModelId = requestItem.EquipmentModelId;

                var transferEquipmentModel = await context.EquipmentModels
                    .Where(equipModel => equipModel.EquipmentModelId == requestItem.EquipmentModelId)
                    .FirstOrDefaultAsync();

                if (transferEquipmentModel == null) return NotFound($"Transfer Item Model with Id {requestItem.EquipmentModelId} Not Found");

                transferItem.Cost = transferEquipmentModel.Cost;

                transferItems.Add(transferItem);
            }

            transfer.TransferItems = transferItems;

            context.Transfers.Add(transfer);
            await context.SaveChangesAsync();

            await notificationManager.Add(type: NOTIFICATIONTYPE.TRANSFER,
                status: transfer.Status,
                actionId: transfer.TransferId,
                siteId: transfer.SendSiteId,
                employeeId: transfer.RequestedById);

            return Ok(transfer.TransferId);
        }

        [HttpPost("request/material")]
        public async Task<ActionResult<List<Transfer>>> RequestMaterial(CreateMaterialTransferDTO transferDTO)
        {
            Transfer transfer = new();
            transfer.SendSiteId = transferDTO.SendSiteId;
            transfer.ReceiveSiteId = transferDTO.ReceiveSiteId;
            transfer.RequestedById = transferDTO.RequestedById;

            ICollection<TransferItem> transferItems = new List<TransferItem>();

            foreach (var requestItem in transferDTO.TransferItems)
            {
                TransferItem transferItem = new();
                transferItem.ItemId = requestItem.ItemId;
                transferItem.QtyRequested = requestItem.QtyRequested;
                transferItem.RequestRemark = requestItem.RequestRemark;

                var itemTemp = await context.Items.Where(item => item.ItemId == requestItem.ItemId).
                    Include(i => i.Equipment).
                    Include(i => i.Material).
                    FirstOrDefaultAsync();

                if (itemTemp == null) return NotFound($"Transfer Item with Id {requestItem.ItemId} Not Found");

                if (itemTemp.Type != ITEMTYPE.MATERIAL) return BadRequest($"Transfer Item with Id {requestItem.ItemId} Is Not Type of Material");

                transferItem.Cost = itemTemp.Material.Cost;

                transferItems.Add(transferItem);
            }

            transfer.TransferItems = transferItems;

            context.Transfers.Add(transfer);
            await context.SaveChangesAsync();

            await  notificationManager.Add(type: NOTIFICATIONTYPE.TRANSFER,
                 status: transfer.Status,
                 actionId: transfer.TransferId,
                 siteId: transfer.SendSiteId,
                 employeeId: transfer.RequestedById);
            return Ok(transfer.TransferId);
        }

        [HttpPost("approve")]
        public async Task<ActionResult<Transfer>> Approve(ApproveTransferDTO approveDTO)
        {
            var transfer = await context.Transfers
                .Where(t => t.TransferId == approveDTO.TransferId)
                .Include(t => t.TransferItems)
                .ThenInclude(ti => ti.Item)
                .FirstOrDefaultAsync();
            if (transfer == null) return NotFound("Transfer Not Found.");

            transfer.ApproveDate = DateTime.Now;
            transfer.ApprovedById = approveDTO.ApprovedById;

            foreach (var requestItem in approveDTO.TransferItems)
            {
                var transferItem = transfer.TransferItems
                    .Where(ti => ti.ItemId == requestItem.ItemId && ti.EquipmentModelId == requestItem.EquipmentModelId)
                    .FirstOrDefault();

                if (transferItem == null) return NotFound($"Transfer Item with Id {requestItem.ItemId} Not Found");

                transferItem.QtyApproved = requestItem.QtyApproved;
                transferItem.ApproveRemark = requestItem.ApproveRemark;
            }

            transfer.Status = TRANSFERSTATUS.APPROVED;

            await context.SaveChangesAsync();

            await notificationManager.Add(type: NOTIFICATIONTYPE.TRANSFER,
                 status: transfer.Status,
                 actionId: transfer.TransferId,
                 siteId: transfer.SendSiteId,
                 employeeId: transfer.RequestedById);
            return Ok(transfer);
        }

        [HttpPost("decline")]
        public async Task<ActionResult<Transfer>> Decline(DeclineTransferDTO declineDTO)
        {
            var transfer = await context.Transfers
               .Where(t => t.TransferId == declineDTO.TransferId)
               .Include(t => t.TransferItems)
               .FirstOrDefaultAsync();
            if (transfer == null) return NotFound("Transfer Not Found.");

            transfer.ApproveDate = DateTime.Now;
            transfer.ApprovedById = declineDTO.ApprovedById;

            foreach (var requestItem in declineDTO.TransferItems)
            {
                var transferItem = transfer.TransferItems
                    .Where(ti => ti.ItemId == requestItem.ItemId && ti.EquipmentModelId == requestItem.EquipmentModelId)
                    .FirstOrDefault();
                if (transferItem == null) return NotFound($"Transfer Item with Id {requestItem.ItemId} Not Found");

                transferItem.ApproveRemark = requestItem.ApproveRemark;
            }

            transfer.Status = TRANSFERSTATUS.DECLINED;

            await context.SaveChangesAsync();

            await notificationManager.Add(type: NOTIFICATIONTYPE.TRANSFER,
                status: transfer.Status,
                actionId: transfer.TransferId,
                siteId: transfer.SendSiteId,
                employeeId: transfer.RequestedById);
            return Ok(transfer);
        }

        [HttpPost("send")]
        public async Task<ActionResult<Transfer>> Send(SendTransferDTO sendDTO)
        {
            var transfer = context.Transfers
                .Where(t => t.TransferId == sendDTO.TransferId)
                .Include(t => t.TransferItems)
                .ThenInclude(ti => ti.Item)
                .FirstOrDefault();
            if (transfer == null) return NotFound("Transfer Not Found.");

            transfer.SendDate = DateTime.Now;
            transfer.SentById = sendDTO.SentById;

            foreach (var requestItem in sendDTO.TransferItems)
            {
                var transferItem = transfer.TransferItems
                    .Where(ti => ti.ItemId == requestItem.ItemId && ti.EquipmentModelId == requestItem.EquipmentModelId)
                    .FirstOrDefault();
                if (transferItem == null) 
                    return NotFound($"Transfer Item with Item Id {requestItem.ItemId} and Model Id {requestItem.EquipmentModelId} Not Found");

                transferItem.SendRemark = requestItem.SendRemark;

                if (transferItem.Item.Type == ITEMTYPE.EQUIPMENT && requestItem.EquipmentAssets != null)
                {
                    transferItem.TransferEquipmentAssets = new List<TransferItemEquipmentAsset>();

                    foreach (var requestAsset in requestItem.EquipmentAssets)
                    {
                        TransferItemEquipmentAsset equipmentAsset = new();
                        equipmentAsset.EquipmentAssetId = requestAsset.EquipmentAssetId;

                        transferItem.TransferEquipmentAssets.Add(equipmentAsset);
                    }
                }
            }

            transfer.Status = TRANSFERSTATUS.SENT;

            await context.SaveChangesAsync();

            await notificationManager.Add(type: NOTIFICATIONTYPE.TRANSFER,
                status: transfer.Status,
                actionId: transfer.TransferId,
                siteId: transfer.SendSiteId,
                employeeId: transfer.RequestedById);

            return Ok(transfer);
        }


        [HttpPost("receive")]
        public async Task<ActionResult<Transfer>> Receive(ReceiveTransferDTO receiveDTO)
        {
            var transfer = context.Transfers
                .Where(t => t.TransferId == receiveDTO.TransferId)
                .Include(t => t.TransferItems)
                .FirstOrDefault();
            if (transfer == null) return NotFound("Transfer Not Found.");

            transfer.ReceiveDate = DateTime.Now;
            transfer.ReceivedById = receiveDTO.ReceivedById;
            transfer.DeliveredById = receiveDTO.DeliveredById;
            transfer.VehiclePlateNo = receiveDTO.VehiclePlateNo;


            foreach (var requestItem in receiveDTO.TransferItems)
            {
                var transferItem = transfer.TransferItems
                    .Where(ti => ti.ItemId == requestItem.ItemId && ti.EquipmentModelId == requestItem.EquipmentModelId)
                    .FirstOrDefault();
                if (transferItem == null) return NotFound($"Transfer Item with Id {requestItem.ItemId} and Model Id {requestItem.EquipmentModelId} Not Found");

                transferItem.ReceiveRemark = requestItem.ReceiveRemark;
            }

            transfer.Status = TRANSFERSTATUS.RECEIVED;

            await context.SaveChangesAsync();


            return Ok(transfer);
        }

    }
}
