using ERP.Context;
using ERP.Models;
using Microsoft.EntityFrameworkCore;

namespace ERP.Controllers
{
    public class NotificationManager
    {
        private readonly DataContext context;
        private Notification notification;

        public NotificationManager(DataContext context)
        {
            this.context = context;
        }

        public async Task<bool> Add(string type, int status, int actionId, int siteId, int employeeId)
        {
            //If there are previous notifications for the same transfer/issue/receive
            await Clear(type, actionId);

            notification = new();
            notification.Type = type;
            notification.Status = status;
            notification.ActionId = actionId;
            notification.SiteId = siteId;
            notification.EmployeeId = employeeId;
            await MakeContent(type, status, actionId);

            context.Notifications.Add(notification);
            await context.SaveChangesAsync();

            return true;

        }

        private async Task Clear(string type, int actionId)
        {
            List<Notification> notifications = await context.Notifications
                .Where(n => n.Type == type && n.ActionId == actionId)
                .ToListAsync();

           if (notifications.Count == 0) return;

            foreach (var notification in notifications)
            {
                notification.IsCleared = true;
            }

            await context.SaveChangesAsync();

        }

        public async Task<bool> Clear(int notificationId)
        {
            var notification = await context.Notifications
                .Where(n => n.NotificationId == notificationId)
                .FirstOrDefaultAsync();

            if (notification != null)
            {
                notification.IsCleared = true;
                await context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<List<Notification>> Get(string type, int status, int siteId)
        {
            var notifications = await context.Notifications
                .Where(n => n.Type == type && n.Status == status && n.SiteId == siteId && n.IsCleared == false)
                .ToListAsync();

            return notifications;
        }

        private async Task MakeContent(string type, int status, int actionId)
        {
            switch (type)
            {
                case NOTIFICATIONTYPE.TRANSFER:
                    await MakeContentTransfer(status, actionId);
                    break;

                case NOTIFICATIONTYPE.ISSUE:
                    await MakeContentIssue(status, actionId);
                    break;

                case NOTIFICATIONTYPE.PURCHASE:
                    await MakeContentPurchase(status, actionId);
                    break;

                case NOTIFICATIONTYPE.BORROW:
                    await MakeContentBorrow(status, actionId);
                    break;

                case NOTIFICATIONTYPE.RECEIVE:
                    await MakeContentReceive(status, actionId);
                    break;

                case NOTIFICATIONTYPE.MAINTAINANCE:
                    await MakeContentMaintainance(status, actionId);
                    break;



            }
        }

        private async Task MakeContentTransfer(int status, int actionId)
        {
            switch (status)
            {
                case TRANSFERSTATUS.REQUESTED:
                    notification.Title = "Approve Transfer Request";
                    notification.Content = $"New Transfer Request With Transfer No: {actionId}";
                    break;

                case TRANSFERSTATUS.DECLINED:
                    notification.Title = "Declined Transfer Request";
                    notification.Content = $"Transfer Request {actionId} Has Been Declined";
                    break;

                case TRANSFERSTATUS.APPROVED:
                    notification.Title = "Transfer Out Items";
                    notification.Content = $"Transfer Request {actionId} Has Been Approved";
                    break;

                case TRANSFERSTATUS.SENT:
                    notification.Title = "Transfer In Items";
                    notification.Content = $"Transfer Request {actionId} Items Has Been Sent";
                    break;

            }
        }

        private async Task MakeContentIssue(int status, int actionId)
        {
            switch (status)
            {
                case ISSUESTATUS.REQUESTED:
                    notification.Title = "Approve Issue";
                    notification.Content = $"New Issue Request With Issue No: {actionId}";
                    break;

                case ISSUESTATUS.APPROVED:
                    notification.Title = "Hand Over Issued Items";
                    notification.Content = $"Issue Request {actionId} Has Been Approved";
                    break;

                case ISSUESTATUS.HANDED:
                    notification.Title = "Issued Items Handed Over";
                    notification.Content = $"Issue Request {actionId} Items Has Been Handed Over";
                    break;

                case ISSUESTATUS.DECLINED:
                    notification.Title = "Declined Transfer Request";
                    notification.Content = $"Issue Request {actionId} Has Been Declined";
                    break;

            }
        }

        private async Task MakeContentPurchase(int status, int actionId)
        {
            switch (status)
            {
                case PurchaseStatus.Requested:
                    notification.Title = "Approve Purchase Request";
                    notification.Content = $"New Purchase Request With Request No: {actionId}";
                    break;

                case PurchaseStatus.Approved:
                    notification.Title = "Check Purchased Items";
                    notification.Content = $"Check Purchase Request {actionId} Items";
                    break;

                case PurchaseStatus.Declined:
                    notification.Title = "Declined Purchase Request";
                    notification.Content = $"Purchase Request {actionId} Has Been Declined";
                    break;

            }
        }

        private async Task MakeContentReceive(int status, int actionId)
        {
            switch (status)
            {
                case ReceiveStatus.Received:
                    notification.Title = "Check Received Items";
                    notification.Content = $"Items of Receive No. {actionId} Has Been Received";
                    break;

            }
        }

        private async Task MakeContentBorrow(int status, int actionId)
        {
            switch (status)
            {
                case BORROWSTATUS.REQUESTED:
                    notification.Title = "Approve Borrow Request";
                    notification.Content = $"New Borrow Request With Borrow No: {actionId}";
                    break;

                case BORROWSTATUS.APPROVED:
                    notification.Title = "Hand Over Borrow Request Items";
                    notification.Content = $"Borrow Request {actionId} Has Been Approved";
                    break;

                case BORROWSTATUS.HANDED:
                    notification.Title = "Borrowed Items Handed Over";
                    notification.Content = $"Borrow Request {actionId} Items Has Been Handed Over";
                    break;

                case BORROWSTATUS.DECLINED:
                    notification.Title = "Declined Borrow Request";
                    notification.Content = $"Borrow Request {actionId} Has Been Declined";
                    break;

            }
        }

        private async Task MakeContentMaintainance(int status, int actionId)
        {
            switch (status)
            {
                case MAINTENANCESTATUS.REQUESTED:
                    notification.Title = "Approve Maintenance Request";
                    notification.Content = $"New Maintainance Request With Request No: {actionId}";
                    break;

                case MAINTENANCESTATUS.APPROVED:
                    notification.Title = "Maintenance Request Approved";
                    notification.Content = $"Item on Maintenance Request {actionId} Has Been Approved";
                    break;

                case MAINTENANCESTATUS.FIXED:
                    notification.Title = "Maintenance Request Item Fixed";
                    notification.Content = $"Item on Maintenance Request {actionId} Has Been Fixed";
                    break;

                case MAINTENANCESTATUS.DECLINED:
                    notification.Title = "Declined Mainenance Request";
                    notification.Content = $"Maintenance Request {actionId} Has Been Declined";
                    break;

            }
        }
    }
}
