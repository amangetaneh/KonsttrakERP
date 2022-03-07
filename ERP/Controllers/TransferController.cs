using ERP.Context;
using ERP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.Controllers
{

    [Route("[controller]")]
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

            var transfers = await context.Transfers.ToListAsync();

            return Ok(transfers);
        }

        [HttpPost("request")]
        public async Task<ActionResult<List<Transfer>>> Post(Transfer transfer)
        {

            context.Transfers.Add(transfer);
            await context.SaveChangesAsync();

            return Ok(context.Transfers.FindAsync(transfer.Id));
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
