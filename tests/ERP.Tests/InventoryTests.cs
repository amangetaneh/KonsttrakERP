using ERP.Models;
using ERP.Services.ItemSiteQtyServices;
using ERP.Services.NotificationServices;

namespace ERP.Tests
{
    public class InventoryTests
    {
        [Fact]
        public async Task SubtractionNeverMakesStockNegativeAndRaisesMinimumNotification()
        {
            using var context = TestContext.Create();
            context.MaterialSiteQties.Add(new MaterialSiteQty { ItemId = 1, SiteId = 1, Qty = 3, MinimumQty = 2 });
            await context.SaveChangesAsync();
            var notifications = new RecordingNotifications();
            var result = await new ItemSiteQtyService(context, notifications).SubtractMaterial(1, 1, 9);
            Assert.Equal(0, result.Qty);
            Assert.Equal((NOTIFICATIONTYPE.MINMATERIAL, 1), Assert.Single(notifications.Added));
        }

        [Fact]
        public async Task ReplenishmentAtMinimumClearsEquipmentNotification()
        {
            using var context = TestContext.Create();
            context.EquipmentSiteQties.Add(new EquipmentSiteQty { EquipmentModelId = 1, SiteId = 2, Qty = 0, MinimumQty = 2 });
            await context.SaveChangesAsync();
            var notifications = new RecordingNotifications();
            var service = new ItemSiteQtyService(context, notifications);
            var result = await service.AddEquipmentModel(1, 2, 2);
            Assert.Equal(2, result.Qty);
            Assert.Equal((NOTIFICATIONTYPE.MINEQUIPMENT, 1), Assert.Single(notifications.Cleared));
        }

        [Fact]
        public async Task RejectsNegativeQuantitiesAndMissingStockEntries()
        {
            using var context = TestContext.Create();
            var service = new ItemSiteQtyService(context, new RecordingNotifications());
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.AddMaterial(1, 1, -1));
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.SubtractMaterial(1, 1, -1));
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.AddEquipmentModel(1, 1, -1));
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.SubtractEquipmentModel(1, 1, -1));
            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.AddMaterial(1, 1, 1));
        }
    }

    internal sealed class RecordingNotifications : INotificationService
    {
        public List<(string, int)> Added { get; } = [];
        public List<(string, int)> Cleared { get; } = [];
        public Task<bool> Add(string type, int status, int actionId, int siteId, int? employeeId)
        {
            Added.Add((type, actionId));
            return Task.FromResult(true);
        }
        public Task Clear(string type, int actionId) { Cleared.Add((type, actionId)); return Task.CompletedTask; }
        public Task<bool> Clear(int notificationId) => throw new NotSupportedException();
        public Task<List<Notification>> Get(string type, int status, int siteId) => throw new NotSupportedException();
        public Task<List<Notification>> GetAllNotifications() => throw new NotSupportedException();
        public Task<Notification?> GetNotification(int id) => throw new NotSupportedException();
        public Task<List<Notification>> GetUserNotifications() => throw new NotSupportedException();
    }
}
