using ERP.DTOs;
using ERP.Models;
using ERP.Services.ReportServices;
using Microsoft.EntityFrameworkCore;

namespace ERP.Tests
{
    public class ReportTests
    {
        public static IEnumerable<object[]> MissingEventGroups()
        {
            var events = new Dictionary<string, string[]>
            {
                ["purchase"] = ["Approve", "Check", "Purchase"],
                ["receive"] = ["Approve", "Receive", "Purchase"],
                ["issue"] = ["Approve", "Hand"],
                ["transfer"] = ["Approve", "Send", "Receive"],
                ["borrow"] = ["Approve", "Hand"],
                ["maintenance"] = ["Approve", "Fix"]
            };
            foreach (var (kind, names) in events)
                foreach (var name in names)
                    foreach (var part in new[] { "Date", "Week", "Month", "Year" })
                        yield return [kind, name + part];
        }

        [Theory]
        [MemberData(nameof(MissingEventGroups))]
        public async Task OptionalEventDatesProduceNotAvailableSummaries(string kind, string group)
        {
            using var context = TestContext.Create();
            var site = new Site { SiteId = 1, Name = "Demo Site" };
            var role = new UserRole { RoleId = 1, Role = "Demo Role" };
            var employee = new Employee { EmployeeId = 1, FName = "Demo", UserRole = role, EmployeeSite = site };
            var purchase = new Purchase { PurchaseId = 1, RequestedBy = employee, ReceivingSite = site };
            context.AddRange(purchase,
                new Receive { ReceiveId = 1, Purchase = purchase, ReceivingSite = site },
                new Issue { IssueId = 1, RequestedBy = employee, Site = site },
                new Transfer { TransferId = 1, RequestedBy = employee, SendSite = site, ReceiveSite = site },
                new Borrow { BorrowId = 1, RequestedBy = employee, Site = site },
                new Maintenance { MaintenanceId = 1, RequestedBy = employee, Site = site });
            await context.SaveChangesAsync();
            var reports = new ReportService(context, new GeneralReportService(context));
            IQueryable? summary = kind switch
            {
                "purchase" => (await reports.GetPurchaseReport(new PurchaseReportDTO { GroupBy = group })).Summary,
                "receive" => (await reports.GetReceiveReport(new ReceiveReportDTO { GroupBy = group })).Summary,
                "issue" => (await reports.GetIssueReport(new IssueReportDTO { GroupBy = group })).Summary,
                "transfer" => (await reports.GetTansferReport(new TransferReportDTO { GroupBy = group })).Summary,
                "borrow" => (await reports.GetBorrowReport(new BorrowReportDTO { GroupBy = group })).Summary,
                "maintenance" => (await reports.GetMaintenanceReport(new MaintenanceReportDTO { GroupBy = group })).Summary,
                _ => throw new ArgumentException(nameof(kind))
            };
            Assert.NotNull(summary);
            var row = Assert.Single(summary.Cast<object>());
            Assert.Equal("N/A", row.GetType().GetProperty("value")!.GetValue(row));
        }

        [Theory]
        [InlineData("Date")]
        [InlineData("Week")]
        [InlineData("Month")]
        [InlineData("Year")]
        public async Task GeneralPurchaseReportPreservesQuantityWhenTheEventDateIsMissing(string group)
        {
            using var context = TestContext.Create();
            var site = new Site { Name = "Demo Site" };
            var employee = new Employee { FName = "Demo", UserRole = new UserRole { Role = "Demo Role" }, EmployeeSite = site };
            var item = new Item { Name = "Demo Material", Type = ITEMTYPE.MATERIAL, Material = new Material { Unit = "each", Cost = 10 } };
            context.PurchaseItems.Add(new PurchaseItem
            {
                Purchase = new Purchase { Status = PURCHASESTATUS.PURCHASED, RequestedBy = employee, ReceivingSite = site },
                Item = item,
                QtyPurchased = 2,
                Cost = 10
            });
            await context.SaveChangesAsync();
            var result = await new GeneralReportService(context).GetPurchaseReport(new GeneralReportDTO { GroupBy = group });
            var summary = Assert.Single(result);
            Assert.Equal("N/A", (string)summary.Label);
            Assert.Equal(2m, summary.Qty);
            Assert.Equal(20m, summary.Cost);
        }

        [Fact]
        public async Task ReadingReturnReportsDoesNotRemoveEmployeeRoles()
        {
            using var context = TestContext.Create();
            var role = new UserRole { Role = "Demo Role" };
            var employee = new Employee { FName = "Demo", UserRole = role };
            context.Returns.Add(new Return { ReturnedBy = employee, Site = new Site { Name = "Demo Site" } });
            await context.SaveChangesAsync();
            var reports = new ReportService(context, new GeneralReportService(context));
            Assert.Single((await reports.GetReturnReport(new ReturnReportDTO())).Data);
            context.ChangeTracker.DetectChanges();
            Assert.Same(role, employee.UserRole);
            Assert.All(context.ChangeTracker.Entries(), entry => Assert.Equal(EntityState.Unchanged, entry.State));
        }
    }
}
