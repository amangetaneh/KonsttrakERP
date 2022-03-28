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
    //Uncomment the line below to enable authorization
    //[Authorize(Roles = "Employee")]
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

        [HttpPost("receive")]
        public async Task<ActionResult<List<Receive>>> Post(CreateReceiveDTO receiveDTO)
        {
            var purchase = context.Purchases.Where(purchase => purchase.PurchaseId == receiveDTO.PurchaseId)
              .Include(purchase => purchase.PurchaseItems)
              .ThenInclude(purchaseItem => purchaseItem.Item)
              .ThenInclude(item => item.Material)
              .Include(purchase => purchase.PurchaseItems)
              .ThenInclude(purchaseItem => purchaseItem.Item.Equipment)
              .FirstOrDefault();

            var purchaseItem = context.PurchaseItems
                .Where(pItem => pItem.PurchaseId == receiveDTO.PurchaseId && pItem.ItemId == receiveDTO.ItemId);

            if (purchase == null) return NotFound("Purchase Not Found.");

            Receive receive = new();
            receive.PurchaseId = purchase.PurchaseId;
            receive.DeliveredById = receiveDTO.DeliveredById;
            receive.DeliveredFrom = receiveDTO.DeliveredFrom;
            receive.ReceivedById = Employee.EmployeeId;

            context.Receives.Add(receive);

            await context.SaveChangesAsync();

            ReceiveItem receiveItem = new();
            receiveItem.ReceiveId = receive.ReceiveId;
            receiveItem.ItemId = receiveDTO.ItemId;
            receiveItem.QtyReceived = receiveDTO.QtyReceived;
            receiveItem.ReceiveRemark = receiveDTO.ReceiveRemark;


            //if (itemTemp == null) return NotFound($"Item with Id {requestItem.ItemId} Not Found");

            //if (itemTemp.Type == ITEMTYPE.MATERIAL)
               // receiveItem.Cost = itemTemp.Material.Cost;

            //else
            //////////////////////////////////////////////////////

            //receiveItems.Add(receiveItem);
            

            //receive.ReceiveItems = receiveItems;

            
            return Ok(receive);
        }

        [HttpPost("approve")]
        public async Task<ActionResult<Receive>> Approve(ApproveReceiveDTO request)
        {
            var receive = await context.Receives.FindAsync(request.ReceieveId);

            if (receive == null) return NotFound("Request Not Found.");

            receive.ApproveDate = DateTime.Now;
            receive.ApprovedById = request.ApprovedById;
            receive.Status = RECEIVESTATUS.APPROVED;

            await context.SaveChangesAsync();

            return Ok(receive);
        }
    }
}
