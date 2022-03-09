using ERP.Context;
using ERP.DTOs;
using ERP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PurchaseItem = ERP.Models.PurchaseItem;

namespace ERP.Controllers
{

    [Route("/api/[controller]")]
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
               .Include(purchase => purchase.RequestStore)
               .ThenInclude(store => store.Site)
               .Include(purchase => purchase.RequestedBy)
               .Include(purchase => purchase.CheckedBy)
               .Include(purchase => purchase.ApprovedBy)
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
            purchase.RequestStoreId = purchaseDTO.RequestStoreId;
            purchase.RequestedById = purchaseDTO.RequestedById;

            ICollection<PurchaseItem> purchaseItems = new List<PurchaseItem>();

            foreach (var requestItem in purchaseDTO.PurchaseItems)
            {
                PurchaseItem purchaseItem = new();
                purchaseItem.ItemId = requestItem.ItemId;
                purchaseItem.QtyRequested = requestItem.QtyRequested;
                purchaseItem.Remark = requestItem.Remark;

                var itemTemp = context.Items.Where(item => item.Id == requestItem.ItemId).
                    Include(i => i.NonConsumableItem).
                    Include(i => i.ConsumableItem).
                    FirstOrDefault();

                if (itemTemp == null) return NotFound($"Purchase Item with Id {requestItem.ItemId} Not Found");

                if (itemTemp.Type == ItemType.Consumable)
                {
                    purchaseItem.Cost = itemTemp.ConsumableItem.Cost;
                    purchaseItem.TotalCost = itemTemp.ConsumableItem.Cost * purchaseItem.QtyRequested;
                }
                else
                {
                    purchaseItem.Cost = itemTemp.NonConsumableItem.Cost;
                    purchaseItem.TotalCost = itemTemp.NonConsumableItem.Cost * purchaseItem.QtyRequested;
                }

                purchaseItems.Add(purchaseItem);
            }

            purchase.PurchaseItems = purchaseItems;

            context.Purchases.Add(purchase);
            await context.SaveChangesAsync();

            return Ok(purchase);
        }

        [HttpPost("approve")]
        public async Task<ActionResult<Purchase>> Approve(ApprovePurchaseDTO request)
        {
            var purchase = context.Purchases
                 .Where(t => t.Id == request.PurchaseId)
                 .Include(t => t.PurchaseItems)
                 .FirstOrDefault();
            if (purchase == null) return NotFound("Request Not Found.");

            purchase.ApproveDate = DateTime.Now;
            purchase.ApprovedById = request.ApprovedById;

            foreach (var requestItem in request.PurchaseItems)
            {
                var purchaseItem = purchase.PurchaseItems.Where(tItem => tItem.ItemId == requestItem.ItemId).FirstOrDefault();

                if (purchaseItem == null) return NotFound($"Purchase Item with Id {requestItem.ItemId} Not Found");

                purchaseItem.QtyApproved = requestItem.QtyApproved;

                purchaseItem.TotalCost = purchaseItem.Cost * purchaseItem.QtyApproved;
            }

            purchase.Status = PurchaseStatus.Approved;

            await context.SaveChangesAsync();

            return Ok(purchase);
        }
        
        [HttpPost("check")]
        public async Task<ActionResult<Purchase>> Check(CheckPurchaseDTO request)
        {
            var purchase = await context.Purchases.FindAsync(request.PurchaseId);

            if (purchase == null) return NotFound("Request Not Found.");

            purchase.CheckDate = DateTime.Now;
            purchase.CheckedById = request.CheckedById;
            purchase.Status = PurchaseStatus.Checked;

            await context.SaveChangesAsync();

            return Ok(purchase);
        }

    }
}
