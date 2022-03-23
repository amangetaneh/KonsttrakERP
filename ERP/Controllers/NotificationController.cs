using ERP.Context;
using ERP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : Controller
    {
        private readonly DataContext context;
        private readonly NotificationManager notificationManager;

        public NotificationController(DataContext context, NotificationManager notificationManager)
        {
            this.context = context;
            this.notificationManager = notificationManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<Notification>>> Get()
        {
            var notifications = await context.Notifications.Where(n => n.IsCleared == false).ToListAsync();

            return Ok(notifications);
        }

        [HttpGet("clear/{notificationId}")]
        public async Task<ActionResult<bool>> Clear(int notificationId)
        {
            var res = await notificationManager.Clear(notificationId);
            return Ok(res);
        }
    }
}
