using ERP.Context;
using ERP.Models;
using ERP.Services.NotificationServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Employee")]
    public class NotificationController : Controller
    {
        private readonly DataContext _context;
        private readonly INotificationService _notificationService;

        public NotificationController(DataContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Notification>>> Get()
        {
            List<Notification> notifications = await _context.Notifications.Where(n => n.IsCleared == false).ToListAsync();

            notifications = notifications.OrderByDescending(n => n.Date).ToList();

            return Ok(notifications);
        }

        [HttpGet("clear/{notificationId}")]
        public async Task<ActionResult<bool>> Clear(int notificationId)
        {
            var res = await _notificationService.Clear(notificationId);
            return Ok(res);
        }

        [HttpGet("now")]
        public async Task<ActionResult<string>> Now()
        {
            return Ok(DateTime.Now.ToString());
        }
    }
}
