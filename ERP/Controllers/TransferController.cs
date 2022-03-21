using ERP.Context;
using ERP.DTOs;
using ERP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransferItem = ERP.Models.TransferItem;

namespace ERP.Controllers
{

    [Route("/api/[controller]")]
    [ApiController]
    public class TransferController : Controller
    {
        private readonly DataContext context;

        public TransferController(DataContext context)
        {
            this.context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Transfer>> Get(int id)
        {
            var transfer = context.Transfers.Where(transfer => transfer.TransferId == id)
                .Include(transfer => transfer.SendStore)
                .ThenInclude(store => store.Site)
                .Include(transfer => transfer.ReceiveStore)
                .ThenInclude(store => store.Site)
                .Include(transfer => transfer.RequestedBy)
                .Include(transfer => transfer.SentBy)
                .Include(transfer => transfer.ReceivedBy)
                .Include(transfer => transfer.ApprovedBy)
               .Include(transfer => transfer.TransferItems)
               .ThenInclude(transferItem => transferItem.Item)
               .ThenInclude(item => item.Material)
               .Include(transfer => transfer.TransferItems)
               .ThenInclude(transferItem => transferItem.Item.Equipment)
               .FirstOrDefault();

            if (transfer == null) return NotFound("Transfer Not Found.");

            return Ok(transfer);
        }

        [HttpGet]
        public async Task<ActionResult<List<Transfer>>> Get()
        {

            var transfers = await context.Transfers
                .Include(transfer => transfer.SendStore)
                .ThenInclude(store => store.Site)
                .Include(transfer => transfer.ReceiveStore)
                .ThenInclude(store => store.Site)
                .ToListAsync();

            return Ok(transfers);
        }

        [HttpPost("request")]
        public async Task<ActionResult<List<Transfer>>> Post(CreateTransferDTO transferDTO)
        {
            Transfer transfer = new();
            transfer.SendStoreId = transferDTO.SendStoreId;
            transfer.ReceiveStoreId = transferDTO.ReceiveStoreId;
            transfer.RequestedById = transferDTO.RequestedById;

            ICollection<TransferItem> transferItems = new List<TransferItem>();

            foreach (var requestItem in transferDTO.TransferItems)
            {
                TransferItem transferItem = new();
                transferItem.ItemId = requestItem.ItemId;
                transferItem.QtyRequested = requestItem.QtyRequested;
                transferItem.RequestRemark = requestItem.RequestRemark;

                var itemTemp = context.Items.Where(item => item.ItemId == requestItem.ItemId).
                    Include(i => i.Equipment).
                    Include(i => i.Material).
                    FirstOrDefault();

                if (itemTemp == null) return NotFound($"Transfer Item with Id {requestItem.ItemId} Not Found");

                if (itemTemp.Type == ITEMTYPE.MATERIAL)
                    transferItem.Cost = itemTemp.Material.Cost;
                else
                {
                    transferItem.Cost = itemTemp.Equipment.Cost;
                    transferItem.Model = requestItem.Model;
                    transferItem.AssetNo = requestItem.AssetNo;
                    transferItem.SerialNo = requestItem.SerialNo;
                }

                transferItems.Add(transferItem);
            }

            transfer.TransferItems = transferItems;

            context.Transfers.Add(transfer);
            await context.SaveChangesAsync();

            return Ok(transfer.TransferId);
        }

        [HttpPost("approve")]
        public async Task<ActionResult<Transfer>> Approve(ApproveTransferDTO approveDTO)
        {
            var transfer = await context.Transfers
                .Where(t => t.TransferId == approveDTO.TransferId)
                .Include(t => t.TransferItems)
                .FirstOrDefaultAsync();
            if (transfer == null) return NotFound("Transfer Not Found.");

            transfer.ApproveDate = DateTime.Now;
            transfer.ApprovedById = approveDTO.ApprovedById;

            foreach (var requestItem in approveDTO.TransferItems)
            {
                var transferItem = transfer.TransferItems.Where(tItem => tItem.ItemId == requestItem.ItemId).FirstOrDefault();
                if (transferItem == null) return NotFound($"Transfer Item with Id {requestItem.ItemId} Not Found");

                transferItem.QtyApproved = requestItem.QtyApproved;
                transferItem.ApproveRemark = requestItem.ApproveRemark;
            }

            transfer.Status = TRANSFERSTATUS.APPROVED;

            await context.SaveChangesAsync();

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
                var transferItem = transfer.TransferItems.Where(tItem => tItem.ItemId == requestItem.ItemId).FirstOrDefault();
                if (transferItem == null) return NotFound($"Transfer Item with Id {requestItem.ItemId} Not Found");

                transferItem.ApproveRemark = requestItem.ApproveRemark;
            }

            transfer.Status = TRANSFERSTATUS.DECLINED;

            await context.SaveChangesAsync();

            return Ok(transfer);
        }

        [HttpPost("send")]
        public async Task<ActionResult<Transfer>> Send(SendTransferDTO sendDTO)
        {
            var transfer = context.Transfers
                .Where(t => t.TransferId == sendDTO.TransferId)
                .Include(t => t.TransferItems)
                .FirstOrDefault();
            if (transfer == null) return NotFound("Transfer Not Found.");

            transfer.SendDate = DateTime.Now;
            transfer.SentById = sendDTO.SentById;

            foreach (var requestItem in sendDTO.TransferItems)
            {
                var transferItem = transfer.TransferItems.Where(tItem => tItem.ItemId == requestItem.ItemId).FirstOrDefault();
                if (transferItem == null) return NotFound($"Transfer Item with Id {requestItem.ItemId} Not Found");

                transferItem.SendRemark = requestItem.SendRemark;
            }

            transfer.Status = TRANSFERSTATUS.SENT;

            await context.SaveChangesAsync();


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
                var transferItem = transfer.TransferItems.Where(tItem => tItem.ItemId == requestItem.ItemId).FirstOrDefault();
                if (transferItem == null) return NotFound($"Transfer Item with Id {requestItem.ItemId} Not Found");

                transferItem.ReceiveRemark = requestItem.ReceiveRemark;
            }

            transfer.Status = TRANSFERSTATUS.RECEIVED;

            await context.SaveChangesAsync();


            return Ok(transfer);
        }

    }
}
