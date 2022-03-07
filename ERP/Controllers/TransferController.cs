using ERP.Context;
using ERP.DTOs;
using ERP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            transfer.Date = transferDTO.Date;
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

                transferItems.Add(transferItem);
            }

            transfer.TransferItems = transferItems;

            context.Transfers.Add(transfer);
            await context.SaveChangesAsync();

            return Ok(transfer);
        }

        [HttpPost("approve")]
        public async Task<ActionResult<Transfer>> Approve(int id, int approvedById)
        {
            var transfer = await context.Transfers.FindAsync(id);
            if (transfer == null) return NotFound("Transfer Not Found.");

            transfer.ApprovedById = approvedById;
            transfer.Status = 1;

            await context.SaveChangesAsync();

            return Ok(transfer);
        }

        [HttpPost("send")]
        public async Task<ActionResult<Transfer>> Send(Transfer request)
        {
            var transfer = context.Transfers
                .Where(t => t.Id == request.Id)
                .Include(t => t.TransferItems)
                .FirstOrDefault();
            if (transfer == null) return NotFound("Transfer Not Found.");

            transfer.SentById = request.SentById;

            foreach (var requestItem in request.TransferItems)
            {
                var transferItem = transfer.TransferItems.Where(tItem => tItem.ItemId == requestItem.ItemId).FirstOrDefault();
                if (transferItem == null) return NotFound($"Transfer Item with Id {requestItem.ItemId} Not Found");

                transferItem.QtyTransfered = requestItem.QtyTransfered;
                transferItem.SendRemark = requestItem.SendRemark;
            }

            transfer.Status = 2;

            await context.SaveChangesAsync();


            return Ok(transfer);
        }


        [HttpPost("receive")]
        public async Task<ActionResult<Transfer>> Receive(Transfer request)
        {
            var transfer = context.Transfers
                .Where(t => t.Id == request.Id)
                .Include(t => t.TransferItems)
                .FirstOrDefault();
            if (transfer == null) return NotFound("Transfer Not Found.");

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

            transfer.Status = 2;

            await context.SaveChangesAsync();


            return Ok(transfer);
        }

    }
}
