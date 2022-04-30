using ERP.Context;
using ERP.DTOs;
using ERP.Models;
using ERP.Services.ItemSiteQtyServices;
using ERP.Services.NotificationServices;
using ERP.Services.User;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.ReceiveServices
{
    public class ReceiveService : IReceiveService
    {
        private readonly DataContext _context;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        private readonly IItemSiteQtyService _itemSiteQtyService;

        public ReceiveService(DataContext context, INotificationService notificationService, IUserService userService, IItemSiteQtyService itemSiteQtyService)
        {
            _context = context;
            _notificationService = notificationService;
            _userService = userService;
            _itemSiteQtyService = itemSiteQtyService;
        }

        public async Task<Receive> GetById(int id)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanViewPurchase != 1) return Forbid();

            var receive = _context.Receives.Where(receive => receive.ReceiveId == id)
               .Include(receive => receive.ReceivedBy)
               .Include(receive => receive.ApprovedBy)
               .Include(receive => receive.DeliveredBy)
               .Include(receive => receive.ReceiveItems)
               .ThenInclude(receiveItem => receiveItem.Item)
               .ThenInclude(item => item.Material)
               .Include(receive => receive.ReceiveItems)
               .ThenInclude(receiveItem => receiveItem.Item.Equipment)
               .FirstOrDefault();

            if (receive == null) throw new KeyNotFoundException("Receive Not Found.");

            return receive;
        }

        public async Task<List<Receive>> GetMySite()
        {
            //if (UserAccount != null && UserAccount.UserRole.CanViewPurchase != 1) return Forbid();

            var receives = await _context.Receives
              .Where(receive => receive.ReceivingSiteId == (int)_userService.Employee.EmployeeSiteId && receive.Status == RECEIVESTATUS.PURCHASED)
              .Include(receive => receive.ReceiveItems)
              .ThenInclude(receiveItem => receiveItem.Item)
              .ThenInclude(item => item.Material)
              .Include(receive => receive.ReceiveItems)
              .ThenInclude(receiveItem => receiveItem.Item.Equipment)
              .OrderByDescending(receive => receive.ReceiveId)
              .ToListAsync();

            if (receives == null) throw new KeyNotFoundException("Receive Not Found.");

            return receives;
        }

        public async Task<Receive> ReceiveItem(CreateReceiveDTO receiveDTO)
        {

            var receive = _context.Receives
                 .Where(r => r.ReceiveId == receiveDTO.ReceiveId)
                 .Include(r => r.ReceiveItems)
                 .FirstOrDefault();

            if (receive == null) throw new KeyNotFoundException("Receive Not Found.");

            receive.ReceiveDate = DateTime.Now;
            //receive.DeliveredById = receiveDTO.DeliveredById;
            receive.ReceivedById = _userService.Employee.EmployeeId;
            ICollection<ReceiveItem> receiveItems = new List<ReceiveItem>();

            foreach (var requestItem in receiveDTO.ReceiveItems)
            {
                var receiveItem = receive.ReceiveItems
                    .Where(rItem => rItem.ItemId == requestItem.ItemId)
                    .FirstOrDefault();

                if (receiveItem == null) throw new KeyNotFoundException($"Receive Item with Id {requestItem.ItemId} Not Found");

                receiveItem.QtyReceived = requestItem.QtyReceived;
                receiveItem.ReceiveRemark = requestItem.ReceiveRemark;

                receiveItems.Add(receiveItem);

                await _itemSiteQtyService.AddMaterial(requestItem.ItemId, (int)_userService.Employee.EmployeeSiteId, requestItem.QtyReceived);
            }

            receive.Status = RECEIVESTATUS.RECEIVED;

            foreach (var buy in _context.Buys.Where(buy => buy.PurchaseId == receive.PurchaseId && buy.Status == BUYSTATUS.QUEUED))
            {
                buy.Status = BUYSTATUS.BOUGHT;
            }

            //Notify employees who requested the purchase here

            await _context.SaveChangesAsync();

            return receive;
        }

        public async Task<Receive> ApproveReceive(ApproveReceiveDTO approveDTO)
        {
            var receive = await _context.Receives.FindAsync(approveDTO.ReceieveId);

            if (receive == null) throw new KeyNotFoundException("Receive Not Found.");

            receive.ApproveDate = DateTime.Now;
            receive.ApprovedById = _userService.Employee.EmployeeId;
            receive.Status = RECEIVESTATUS.APPROVED;
            receive.ApproveRemark = approveDTO.ApproveRemark;

            await _context.SaveChangesAsync();

            return receive;
        }
    }
}
