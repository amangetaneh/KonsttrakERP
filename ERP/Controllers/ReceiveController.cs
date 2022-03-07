using ERP.Context;
using ERP.DTOs;
using ERP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.Controllers
{

    [Route("[controller]")]
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

        [HttpPost]
        public async Task<ActionResult<List<Receive>>> Post(CreateReceiveDTO receiveDTO)
        {
            Receive receive = new();
            receive.Date = receiveDTO.Date;
            receive.ReceiveStoreId = receiveDTO.ReceiveStoreId;
            receive.PurchaseId = receiveDTO.PurchaseId;
            receive.DeliveredById = receiveDTO.DeliveredById;
            receive.ReceivedById = receiveDTO.ReceivedById;

            ICollection<ReceiveItem> receiveItems = new List<ReceiveItem>();

            foreach (var requestItem in receiveDTO.ReceiveItems)
            {
                ReceiveItem receiveItem = new();
                receiveItem.ItemId = requestItem.ItemId;
                receiveItem.Qty = requestItem.Qty;

                receiveItems.Add(receiveItem);
            }

            receive.ReceiveItems = receiveItems;

            context.Receives.Add(receive);
            await context.SaveChangesAsync();

            return Ok(receive);
        }

    }
}
