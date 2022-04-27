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
    }
}
