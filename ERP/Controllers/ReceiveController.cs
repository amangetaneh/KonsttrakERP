using ERP.Context;
using ERP.DTOs;
using ERP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReceiveItem = ERP.Models.ReceiveItem;

namespace ERP.Controllers
{

    [Route("/api/[controller]")]
    [ApiController]
    public class ReceiveController : Controller
    {
        private readonly DataContext context;

        public ReceiveController(DataContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Receive>>> Get()
        {

            var receives = await context.Receives.ToListAsync();

            return Ok(receives);
        }

        [HttpPost("receive")]
        public async Task<ActionResult<List<Receive>>> Post(CreateReceiveDTO receiveDTO)
        {
            Receive receive = new();
            receive.ReceiveStoreId = receiveDTO.ReceiveStoreId;
            receive.PurchaseId = receiveDTO.PurchaseId;
            receive.DeliveredById = receiveDTO.DeliveredById;
            receive.DeliveredFrom = receiveDTO.DeliveredFrom;
            receive.ReceivedById = receiveDTO.ReceivedById;

            ICollection<ReceiveItem> receiveItems = new List<ReceiveItem>();

            foreach (var requestItem in receiveDTO.ReceiveItems)
            {
                ReceiveItem receiveItem = new();
                receiveItem.ItemId = requestItem.ItemId;
                receiveItem.Qty = requestItem.Qty;
                receiveItem.Remark = requestItem.Remark;

                var itemTemp = context.Items.Where(item => item.ItemId == requestItem.ItemId).
                   Include(i => i.Equipment).
                   Include(i => i.Material).
                   FirstOrDefault();

                if (itemTemp == null) return NotFound($"Item with Id {requestItem.ItemId} Not Found");

                if (itemTemp.Type == ITEMTYPE.MATERIAL)
                    receiveItem.Cost = itemTemp.Material.Cost;
                
                else
                    receiveItem.Cost = itemTemp.Equipment.Cost;

                receiveItems.Add(receiveItem);
            }

            receive.ReceiveItems = receiveItems;

            context.Receives.Add(receive);
            await context.SaveChangesAsync();

            return Ok(receive);
        }

        [HttpPost("approve")]
        public async Task<ActionResult<Receive>> Approve(ApproveReceiveDTO request)
        {
            var receive = await context.Receives.FindAsync(request.ReceieveId);

            if (receive == null) return NotFound("Request Not Found.");

            receive.ApproveDate = DateTime.Now;
            receive.ApprovedById = request.ApprovedById;
            receive.Status = ReceiveStatus.Approved;

            await context.SaveChangesAsync();

            return Ok(receive);
        }
    }
}
