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
            Clear(type, actionId);

            notification = new();
            notification.Type = type;
            notification.Status = status;
            notification.ActionId = actionId;
            notification.SiteId = siteId;
            notification.EmployeeId = employeeId;
            MakeContent(type, status, actionId);

            await context.Notifications.AddAsync(notification);
            await context.SaveChangesAsync();

            return true;

        }

        private async void Clear(string type, int actionId)
        {
            var notification = await context.Notifications
                .Where(n => n.Type == type && n.ActionId == actionId)
                .FirstOrDefaultAsync();

            if (notification != null)
            {
                notification.IsCleared = true;
                await context.SaveChangesAsync();
            }
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

        private void MakeContent(string type, int status, int actionId)
        {
            switch (type)
            {
                case NOTIFICATIONTYPE.TRANSFER:
                    MakeContentTransfer(status, actionId);
                    break;

                case NOTIFICATIONTYPE.ISSUE:
                    MakeContentIssue(status, actionId);
                    break;

                case NOTIFICATIONTYPE.PURCHASE:
                    MakeContentPurchase(status, actionId);
                    break;

                case NOTIFICATIONTYPE.BORROW:
                    MakeContentBorrow(status, actionId);
                    break;

                case NOTIFICATIONTYPE.RECEIVE:
                    MakeContentReceive(status, actionId);
                    break;

                case NOTIFICATIONTYPE.MAINTAINANCE:
                    MakeContentMaintainance(status, actionId);
                    break;



            }
        }

        private void MakeContentTransfer(int status, int actionId)
        {
            switch (status)
            {
                case TRANSFERSTATUS.REQUESTED:
                    notification.Title = "Approve Transfer Request";
                    notification.Content = $"New Transfer Request With Transfer Number: {actionId}";
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

        private void MakeContentIssue(int status, int actionId)
        {
            switch (status)
            {
                case ISSUESTATUS.REQUESTED:
                    notification.Title = "Approve Issue";
                    notification.Content = $"New Issue Request With Issue Number: {actionId}";
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

        private void MakeContentPurchase(int status, int actionId)
        {
            switch (status)
            {
                case PurchaseStatus.Requested:
                    notification.Title = "Approve Purchase Request";
                    notification.Content = $"New Purchase Request With Request Number: {actionId}";
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

        private void MakeContentReceive(int status, int actionId)
        {
            switch (status)
            {
                case ReceiveStatus.Received:
                    notification.Title = "Check Received Items";
                    notification.Content = $"Items of Receive Number {actionId} Has Been Received";
                    break;

            }
        }

        private void MakeContentBorrow(int status, int actionId)
        {

        }

        private void MakeContentMaintainance(int status, int actionId)
        {
            switch (status)
            {
                case MAINTENANCESTATUS.REQUESTED:
                    notification.Title = "Approve Maintenance Request";
                    notification.Content = $"New Maintainance Request With Request Number: {actionId}";
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
