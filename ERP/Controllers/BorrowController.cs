using ERP.Context;
using ERP.DTOs;
using ERP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BorrowController : Controller
    {
        private readonly DataContext context;
        private readonly NotificationManager notificationManager;

        public BorrowController(DataContext context, NotificationManager notificationManager)
        {
            this.context = context;
            this.notificationManager = notificationManager;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Borrow>> Get(int id)
        {
            var borrow = context.Borrows.Where(borrow => borrow.BorrowId == id)
                .Include(borrow => borrow.Site)
                .Include(borrow => borrow.RequestedBy)
                .Include(borrow => borrow.ApprovedBy)
                .Include(borrow => borrow.HandedBy)
               .Include(borrow => borrow.BorrowItems)
               .ThenInclude(borrowItem => borrowItem.Item)
               .ThenInclude(item => item.Equipment)
               .Include(borrow => borrow.BorrowItems)
               .ThenInclude(borrowItem => borrowItem.BorrowEquipmentAssets)
               .ThenInclude(borrowEquipmentAsset => borrowEquipmentAsset.EquipmentAsset)
               .FirstOrDefault();

            if (borrow == null) return NotFound("Borrow Not Found.");

            return Ok(borrow);
        }

        [HttpGet]
        public async Task<ActionResult<List<Borrow>>> Get()
        {

            var borrows = await context.Borrows
                .Include(borrow => borrow.Site)
                .ToListAsync();

            return Ok(borrows);
        }

        [HttpPost("request")]
        public async Task<ActionResult<int>> Post(CreateBorrowDTO borrowDTO)
        {
            Borrow borrow = new();
            borrow.RequestedById = borrowDTO.RequestedById;
            borrow.SiteId = borrowDTO.SiteId;

            ICollection<BorrowItem> borrowItems = new List<BorrowItem>();

            foreach (var requestItem in borrowDTO.BorrowItems)
            {
                BorrowItem borrowItem = new();
                borrowItem.ItemId = requestItem.ItemId;
                borrowItem.QtyRequested = requestItem.QtyRequested;
                borrowItem.RequestRemark = requestItem.RequestRemark;

                var itemTemp = context.Items.Where(item => item.ItemId == requestItem.ItemId).
                   Include(i => i.Equipment).
                   FirstOrDefault();

                if (itemTemp == null) return NotFound($"Borrow Item with Id {requestItem.ItemId} Not Found");

                if (itemTemp.Type != ITEMTYPE.EQUIPMENT) return BadRequest($"Borrow Item with Id {requestItem.ItemId} Is Not Type of Equipment");

                borrowItem.EquipmentModelId = requestItem.EquipmentModelId;

                var borrowEquipmentModel = await context.EquipmentModels
                    .Where(equipModel => equipModel.EquipmentModelId == requestItem.EquipmentModelId)
                    .FirstOrDefaultAsync();

                if (borrowEquipmentModel == null) return NotFound($"Borrow Item Model with Id {requestItem.EquipmentModelId} Not Found");

                borrowItem.Cost = borrowEquipmentModel.Cost;

                borrowItems.Add(borrowItem);
            }

            borrow.BorrowItems = borrowItems;

            context.Borrows.Add(borrow);
            await context.SaveChangesAsync();

            await notificationManager.Add(type: NOTIFICATIONTYPE.BORROW,
                status: borrow.Status,
                actionId: borrow.BorrowId,
                siteId: borrow.SiteId,
                employeeId: borrow.RequestedById);

            return Ok(borrow.BorrowId);
        }

        [HttpPost("approve")]
        public async Task<ActionResult<Borrow>> Approve(ApproveBorrowDTO approveDTO)
        {
            var borrow = await context.Borrows
                .Where(b => b.BorrowId == approveDTO.BorrowId)
                .Include(b => b.BorrowItems)
                .FirstOrDefaultAsync();
            if (borrow == null) return NotFound("Borrow Not Found.");

            borrow.ApproveDate = DateTime.Now;
            borrow.ApprovedById = approveDTO.ApprovedById;

            foreach (var requestItem in approveDTO.BorrowItems)
            {
                var borrowItem = borrow.BorrowItems
                    .Where(bi => bi.ItemId == requestItem.ItemId && bi.EquipmentModelId == requestItem.EquipmentModelId)
                    .FirstOrDefault();

                if (borrowItem == null) return NotFound($"Borrow Item with Id {requestItem.ItemId} and Model Id {requestItem.EquipmentModelId} Not Found");

                borrowItem.QtyApproved = requestItem.QtyApproved;
                borrowItem.ApproveRemark = requestItem.ApproveRemark;
            }

            borrow.Status = BORROWSTATUS.APPROVED;

            await context.SaveChangesAsync();

            await notificationManager.Add(type: NOTIFICATIONTYPE.BORROW,
                status: borrow.Status,
                actionId: borrow.BorrowId,
                siteId: borrow.SiteId,
                employeeId: borrow.RequestedById);

            return Ok(borrow);
        }

        [HttpPost("decline")]
        public async Task<ActionResult<Borrow>> Decline(DeclineBorrowDTO declineDTO)
        {
            var borrow = await context.Borrows
                 .Where(b => b.BorrowId == declineDTO.BorrowId)
                 .Include(b => b.BorrowItems)
                 .FirstOrDefaultAsync();
            if (borrow == null) return NotFound("Borrow Not Found.");

            borrow.ApproveDate = DateTime.Now;
            borrow.ApprovedById = declineDTO.ApprovedById;

            foreach (var requestItem in declineDTO.BorrowItems)
            {
                var borrowItem = borrow.BorrowItems
                    .Where(bi => bi.ItemId == requestItem.ItemId && bi.EquipmentModelId == requestItem.EquipmentModelId)
                    .FirstOrDefault();

                if (borrowItem == null) return NotFound($"Borrow Item with Id {requestItem.ItemId} and Model Id {requestItem.EquipmentModelId} Not Found");

                borrowItem.QtyApproved = requestItem.QtyApproved;
                borrowItem.ApproveRemark = requestItem.ApproveRemark;
            }

            borrow.Status = BORROWSTATUS.DECLINED;

            await context.SaveChangesAsync();

            await notificationManager.Add(type: NOTIFICATIONTYPE.BORROW,
                status: borrow.Status,
                actionId: borrow.BorrowId,
                siteId: borrow.SiteId,
                employeeId: borrow.RequestedById);

            return Ok(borrow);
        }

        [HttpPost("hand")]
        public async Task<ActionResult<Borrow>> Hand(HandBorrowDTO handDTO)
        {
            var borrow = await context.Borrows
                 .Where(b => b.BorrowId == handDTO.BorrowId)
                 .Include(b => b.BorrowItems)
                 .FirstOrDefaultAsync();
            if (borrow == null) return NotFound("Borrow Not Found.");

            borrow.HandDate = DateTime.Now;
            borrow.HandedById = handDTO.HandedById;

            foreach (var requestItem in handDTO.BorrowItems)
            {
                var borrowItem = borrow.BorrowItems
                     .Where(bi => bi.ItemId == requestItem.ItemId && bi.EquipmentModelId == requestItem.EquipmentModelId)
                     .FirstOrDefault();

                if (borrowItem == null)
                    return NotFound($"Borrow Item with Id {requestItem.ItemId} and Model Id {requestItem.EquipmentModelId} Not Found");

                borrowItem.HandRemark = requestItem.HandRemark;

                if (requestItem.EquipmentAssets != null)
                {
                    borrowItem.BorrowEquipmentAssets = new List<BorrowItemEquipmentAsset>();

                    foreach (var requestAsset in requestItem.EquipmentAssets)
                    {
                        BorrowItemEquipmentAsset equipmentAsset = new();
                        equipmentAsset.EquipmentAssetId = requestAsset.EquipmentAssetId;

                        borrowItem.BorrowEquipmentAssets.Add(equipmentAsset);
                    }
                }
            }

            borrow.Status = BORROWSTATUS.HANDED;

            await context.SaveChangesAsync();

            await notificationManager.Add(type: NOTIFICATIONTYPE.BORROW,
                status: borrow.Status,
                actionId: borrow.BorrowId,
                siteId: borrow.SiteId,
                employeeId: borrow.RequestedById);

            return Ok(borrow);
        }


    }
}
