using ERP.Context;
using ERP.DTOs;
using ERP.Models;
using ERP.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using PurchaseItem = ERP.Models.PurchaseItem;

namespace ERP.Controllers
{

    [Route("/api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Employee")]
    public class PurchaseController : Controller
    {
        private readonly DataContext context;
        private readonly IUserService _userService;

        public UserAccount? UserAccount { get; }

        public Employee Employee { get; }

        public PurchaseController(DataContext context, IUserService userService)
        {
            this.context = context;

            _userService = userService;            

            UserAccount = context.UserAccounts
                .Where(u => u.Username == _userService.GetMyName())
                .Include(u => u.UserRole)
                .Include(u => u.Employee)
                .FirstOrDefault();

            Employee = UserAccount.Employee;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Purchase>> Get(int id)
        {
            if (UserAccount != null && UserAccount.UserRole.CanViewPurchase != 1) return Forbid();

            var purchase = context.Purchases.Where(purchase => purchase.PurchaseId == id)
               .Include(purchase => purchase.RequestedBy)
               .Include(purchase => purchase.CheckedBy)
               .Include(purchase => purchase.ApprovedBy)
               .Include(purchase => purchase.PurchaseItems)
               .ThenInclude(purchaseItem => purchaseItem.Item)
               .ThenInclude(item => item.Material)
               .Include(purchase => purchase.PurchaseItems)
               .ThenInclude(purchaseItem => purchaseItem.Item.Equipment)
               .FirstOrDefault();

            if (purchase == null) return NotFound("Purchase Not Found.");

            return Ok(purchase);
        }

        [HttpGet]
        public async Task<ActionResult<List<Purchase>>> Get()
        {
            if (UserAccount != null && UserAccount.UserRole.CanViewPurchase != 1) return Forbid();

            var purchases = await context.Purchases.ToListAsync();

            return Ok(purchases);
        }

        [HttpPost]
        public async Task<ActionResult<List<Purchase>>> Post(CreatePurchaseDTO purchaseDTO)
        {
            if (UserAccount != null && UserAccount.UserRole.CanRequestPurchase != 1) return Forbid();

            Purchase purchase = new();
            purchase.RequestedById = purchaseDTO.RequestedById;

            ICollection<PurchaseItem> purchaseItems = new List<PurchaseItem>();

            foreach (var requestItem in purchaseDTO.PurchaseItems)
            {
                PurchaseItem purchaseItem = new();
                purchaseItem.ItemId = requestItem.ItemId;
                purchaseItem.QtyRequested = requestItem.QtyRequested;
                //purchaseItem.PurchaseRemark = requestItem.PurchaseRemark;

                var itemTemp = context.Items.Where(item => item.ItemId == requestItem.ItemId).
                    Include(i => i.Equipment).
                    Include(i => i.Material).
                    FirstOrDefault();

                if (itemTemp == null) return NotFound($"Purchase Item with Id {requestItem.ItemId} Not Found");

                if (itemTemp.Type == ITEMTYPE.MATERIAL)
                {
                    purchaseItem.Cost = itemTemp.Material.Cost;
                    purchaseItem.TotalCost = itemTemp.Material.Cost * purchaseItem.QtyRequested;
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
            if (UserAccount != null && UserAccount.UserRole.CanApprovePurchase != 1) return Forbid();

            var purchase = context.Purchases
                 .Where(t => t.PurchaseId == request.PurchaseId)
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

            purchase.Status = PURCHASESTATUS.APPROVED;

            await context.SaveChangesAsync();

            return Ok(purchase);
        }

        [HttpPost("check")]
        public async Task<ActionResult<Purchase>> Check(CheckPurchaseDTO request)
        {
            if (UserAccount != null && UserAccount.UserRole.CanCheckPurchase != 1) return Forbid();

            var purchase = await context.Purchases.FindAsync(request.PurchaseId);

            if (purchase == null) return NotFound("Request Not Found.");

            purchase.CheckDate = DateTime.Now;
            purchase.CheckedById = request.CheckedById;
            purchase.Status = PURCHASESTATUS.CHECKED;

            await context.SaveChangesAsync();

            return Ok(purchase);
        }

        [HttpPost("decline")]
        public async Task<ActionResult<Purchase>> Decline(ApprovePurchaseDTO request)
        {
            if (UserAccount != null && UserAccount.UserRole.CanApprovePurchase != 1) return Forbid();

            var purchase = context.Purchases
                  .Where(t => t.PurchaseId == request.PurchaseId)
                  .Include(t => t.PurchaseItems)
                  .FirstOrDefault();

            if (purchase == null) return NotFound("Purchase Not Found.");

            purchase.ApproveDate = DateTime.Now;
            purchase.ApprovedById = request.ApprovedById;

            foreach (var purchaseItem in purchase.PurchaseItems)
            {
                purchaseItem.QtyApproved = 0;

                purchaseItem.TotalCost = 0;
            }

            purchase.Status = TRANSFERSTATUS.DECLINED;

            await context.SaveChangesAsync();

            return Ok(purchase);
        }
    }
}