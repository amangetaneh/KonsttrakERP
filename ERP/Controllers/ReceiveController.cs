using ERP.Context;
using ERP.DTOs;
using ERP.Models;
using ERP.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReceiveItem = ERP.Models.ReceiveItem;

namespace ERP.Controllers
{

    [Route("/api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Employee")]
    public class ReceiveController : Controller
    {
        private readonly DataContext context;
        private readonly IUserService _userService;

        public UserAccount? UserAccount { get; }

        public Employee Employee { get; }


        public ReceiveController(DataContext context, IUserService userService)
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

        [HttpGet]
        public async Task<ActionResult<List<Receive>>> Get()
        {

            var receives = await context.Receives.ToListAsync();

            return Ok(receives);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Receive>> Get(int id)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanViewPurchase != 1) return Forbid();

            var receive = context.Receives.Where(receive => receive.ReceiveId == id)
               .Include(receive => receive.ReceivedBy)
               .Include(receive => receive.ApprovedBy)
               .Include(receive => receive.DeliveredBy)
               .Include(receive => receive.ReceiveItems)
               .ThenInclude(receiveItem => receiveItem.Item)
               .ThenInclude(item => item.Material)
               .Include(receive => receive.ReceiveItems)
               .ThenInclude(receiveItem => receiveItem.Item.Equipment)
               .FirstOrDefault();

            if (receive == null) return NotFound("Receive Not Found.");

            return Ok(receive);
        }

        [HttpGet("purchase")]
        public async Task<ActionResult<List<Purchase>>> GetPurchase()
        {
            //if (UserAccount != null && UserAccount.UserRole.CanViewPurchase != 1) return Forbid();

            var purchases = await context.Purchases
              .Where(purchase => purchase.ReceivingSiteId == Employee.EmployeeSiteId && purchase.Status == PURCHASESTATUS.PURCHASED)
              .Include(purchase => purchase.PurchaseItems)
              .ThenInclude(purchaseItem => purchaseItem.Item)
              .ThenInclude(item => item.Material)
              .Include(purchase => purchase.PurchaseItems)
              .ThenInclude(purchaseItem => purchaseItem.Item.Equipment)
              .ToListAsync();

            if (purchases == null) return NotFound("Not Found.");

            return Ok(purchases);
        }

        [HttpPost("request")]
        public async Task<ActionResult<List<Receive>>> Post(CreateReceiveDTO receiveDTO)
        {
            
            Receive receive = new();
            receive.PurchaseId = receiveDTO.PurchaseId;
            receive.DeliveredById = receiveDTO.DeliveredById;
            receive.ReceivedById = Employee.EmployeeId;

            ICollection<ReceiveItem> receiveItems = new List<ReceiveItem>();

            foreach (var requestItem in receiveDTO.ReceiveItems)
            {
                ReceiveItem receiveItem = new();
                receiveItem.ItemId = requestItem.ItemId;
                receiveItem.QtyReceived = requestItem.QtyReceived;
                receiveItem.ReceiveRemark = requestItem.ReceiveRemark;

                receiveItems.Add(receiveItem);
            }

            receive.ReceiveItems = receiveItems;
            receive.Status = RECEIVESTATUS.RECEIVED;

            context.Receives.Add(receive);

            var buy = context.Buys.Where(buy => buy.PurchaseId == receiveDTO.PurchaseId && buy.Status == 5)
                            .Include(buy => buy.BuyItems)
                            .FirstOrDefault();

            if (buy != null) buy.Status = BUYSTATUS.BOUGHT;

            //Notify employees who requested the purchase here

            await context.SaveChangesAsync();
                        
            return Ok(receive.ReceiveId);
        }

        [HttpPost("approve")]
        public async Task<ActionResult<Receive>> Approve(ApproveReceiveDTO approveDTO)
        {
            var receive = await context.Receives.FindAsync(approveDTO.ReceieveId);

            if (receive == null) return NotFound("Request Not Found.");

            receive.ApproveDate = DateTime.Now;
            receive.ApprovedById = Employee.EmployeeId;
            receive.Status = RECEIVESTATUS.APPROVED;
            receive.ApproveRemark = approveDTO.ApproveRemark;

            await context.SaveChangesAsync();

            return Ok(receive);
        }
    }
}
