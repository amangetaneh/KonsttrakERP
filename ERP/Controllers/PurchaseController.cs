using ERP.Context;
using ERP.DTOs;
using ERP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class PurchaseController : Controller
    {
        private readonly DataContext context;

        public PurchaseController(DataContext context)
        {
            this.context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Purchase>> Get(int id)
        {
            var purchase = context.Purchases.Where(purchase => purchase.Id == id)
               .Include(purchase => purchase.PurchaseItems)
               .ThenInclude(purchaseItem => purchaseItem.Item)
               .ThenInclude(item => item.ConsumableItem)
               .Include(purchase => purchase.PurchaseItems)
               .ThenInclude(purchaseItem => purchaseItem.Item.NonConsumableItem)
               .FirstOrDefault();

            if (purchase == null) return NotFound("Purchase Not Found.");

            return Ok(purchase);
        }

        [HttpGet]
        public async Task<ActionResult<List<Purchase>>> Get()
        {
            var purchases = await context.Purchases.ToListAsync();

            return Ok(purchases);
        }

        [HttpPost]
        public async Task<ActionResult<List<Purchase>>> Post(CreatePurchaseDTO purchaseDTO)
        {
            Purchase purchase = new();
            purchase.Date = purchaseDTO.Date;
            purchase.RequestStoreId = purchaseDTO.RequestStoreId;
            purchase.RequestedById = purchaseDTO.RequestedById;

            ICollection<PurchaseItem> purchaseItems = new List<PurchaseItem>();

            foreach (var requestItem in purchaseDTO.PurchaseItems)
            {
                PurchaseItem purchaseItem = new();
                purchaseItem.ItemId = requestItem.ItemId;
                purchaseItem.QtyRequested = requestItem.QtyRequested;

                purchaseItems.Add(purchaseItem);
            }

            purchase.PurchaseItems = purchaseItems;

            context.Purchases.Add(purchase);
            await context.SaveChangesAsync();

            return Ok(purchase);
        }

    }
}
