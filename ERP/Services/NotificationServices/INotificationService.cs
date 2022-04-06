using ERP.Models;

namespace ERP.Services.NotificationServices
{
    public interface INotificationService
    {
        public Task<bool> Add(string type, int status, int actionId, int siteId, int employeeId);
        
        public Task<bool> Clear(int notificationId);

        public Task<List<Notification>> Get(string type, int status, int siteId);
        Task<List<Notification>> GetUserNotifications();
    }
}
