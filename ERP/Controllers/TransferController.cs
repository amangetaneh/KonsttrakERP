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
            var transfer = context.Transfers.Where(transfer => transfer.Id == id)
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
               .ThenInclude(item => item.ConsumableItem)
               .Include(transfer => transfer.TransferItems)
               .ThenInclude(transferItem => transferItem.Item.NonConsumableItem)
               .FirstOrDefault();

            if (transfer == null) return NotFound("Transfer Not Found.");

            return Ok(transfer);
        }

        [HttpGet]
        public async Task<ActionResult<List<Transfer>>> Get()
        {

            var transfers = await context.Transfers
                .Include(transfer => transfer.SendStore).ThenInclude(store => store.Site)
                .Include(transfer => transfer.ReceiveStore).ThenInclude(store => store.Site)
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

                var itemTemp = context.Items.Where(item => item.Id == requestItem.ItemId).
                    Include(i => i.NonConsumableItem).
                    Include(i => i.ConsumableItem).
                    FirstOrDefault();

                if (itemTemp == null) return NotFound($"Transfer Item with Id {requestItem.ItemId} Not Found");

                if (itemTemp.Type == ItemType.Consumable)
                    transferItem.Cost = itemTemp.ConsumableItem.Cost;
                else
                    transferItem.Cost = itemTemp.NonConsumableItem.Cost;

                transferItems.Add(transferItem);
            }

            transfer.TransferItems = transferItems;

            context.Transfers.Add(transfer);
            await context.SaveChangesAsync();

            return Ok(transfer.Id);
        }

        [HttpPost("approve")]
        public async Task<ActionResult<Transfer>> Approve(ApproveTransferDTO request)
        {
            var transfer = await context.Transfers.FindAsync(request.transferId);
            if (transfer == null) return NotFound("Transfer Not Found.");

            transfer.ApproveDate = DateTime.Now;
            transfer.ApprovedById = request.approvedById;
            transfer.Status = TransferStatus.Approved;

            await context.SaveChangesAsync();

            return Ok(transfer);
        }

        [HttpPost("decline")]
        public async Task<ActionResult<Transfer>> Decline(ApproveTransferDTO request)
        {
            var transfer = await context.Transfers.FindAsync(request.transferId);
            if (transfer == null) return NotFound("Transfer Not Found.");

            transfer.ApproveDate = DateTime.Now;
            transfer.ApprovedById = request.approvedById;
            transfer.Status = TransferStatus.Declined;

            await context.SaveChangesAsync();

            return Ok(transfer);
        }

        [HttpPost("send")]
        public async Task<ActionResult<Transfer>> Send(SendTransferDTO request)
        {
            var transfer = context.Transfers
                .Where(t => t.Id == request.Id)
                .Include(t => t.TransferItems)
                .FirstOrDefault();
            if (transfer == null) return NotFound("Transfer Not Found.");

            transfer.SendDate = DateTime.Now;
            transfer.SentById = request.SentById;

            foreach (var requestItem in request.TransferItems)
            {
                var transferItem = transfer.TransferItems.Where(tItem => tItem.ItemId == requestItem.ItemId).FirstOrDefault();
                if (transferItem == null) return NotFound($"Transfer Item with Id {requestItem.ItemId} Not Found");

                transferItem.QtySent = requestItem.QtySent;
                transferItem.SendRemark = requestItem.SendRemark;
            }

            transfer.Status = TransferStatus.Sent;

            await context.SaveChangesAsync();


            return Ok(transfer);
        }


        [HttpPost("receive")]
        public async Task<ActionResult<Transfer>> Receive(ReceiveTransferDTO request)
        {
            var transfer = context.Transfers
                .Where(t => t.Id == request.Id)
                .Include(t => t.TransferItems)
                .FirstOrDefault();
            if (transfer == null) return NotFound("Transfer Not Found.");

            transfer.ReceiveDate = DateTime.Now;
            transfer.ReceivedById = request.ReceivedById;
            transfer.DeliveredById = request.DeliveredById;
            transfer.VehiclePlateNo = request.VehiclePlateNo;


            foreach (var requestItem in request.TransferItems)
            {
                var transferItem = transfer.TransferItems.Where(tItem => tItem.ItemId == requestItem.ItemId).FirstOrDefault();
                if (transferItem == null) return NotFound($"Transfer Item with Id {requestItem.ItemId} Not Found");

                transferItem.QtyReceived = requestItem.QtyReceived;
                transferItem.ReceiveRemark = requestItem.ReceiveRemark;
            }

            transfer.Status = TransferStatus.Received;

            await context.SaveChangesAsync();


            return Ok(transfer);
        }

    }
}
