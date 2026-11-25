using System.Data.Common;
using ERP.Context;
using ERP.DTOs;
using ERP.Models;
using ERP.Services.ReportServices;
using Microsoft.EntityFrameworkCore;

namespace ERP.SqlServer.Tests
{
    public sealed class SqlServerTests : IClassFixture<SqlServerDatabase>
    {
        private readonly SqlServerDatabase _database;
        public SqlServerTests(SqlServerDatabase database) => _database = database;

        [Fact]
        public async Task AllMigrationsApplyAndMaterialStockPersists()
        {
            await using var context = _database.CreateContext();
            Assert.Empty(await context.Database.GetPendingMigrationsAsync());
            var site = new Site { Name = "Demo SQL Site" };
            var item = new Item { Name = "Demo SQL Material", Type = ITEMTYPE.MATERIAL, Material = new Material { Unit = "each", Cost = 12 } };
            context.MaterialSiteQties.Add(new MaterialSiteQty { Item = item, Site = site, Qty = 5, MinimumQty = 2 });
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();
            var stock = await context.MaterialSiteQties.Include(row => row.Item).SingleAsync(row => row.ItemId == item.ItemId && row.SiteId == site.SiteId);
            Assert.Equal(5, stock.Qty);
            Assert.Equal("Demo SQL Material", stock.Item.Name);
        }

        [Fact]
        public async Task SqlServerRejectsAStockRowWithMissingForeignKeys()
        {
            await using var context = _database.CreateContext();
            context.MaterialSiteQties.Add(new MaterialSiteQty { ItemId = int.MaxValue, SiteId = int.MaxValue, Qty = 1 });
            await Assert.ThrowsAsync<DbUpdateException>(() => context.SaveChangesAsync());
        }

        [Fact]
        public async Task RolledBackChangesDoNotPersist()
        {
            await using var context = _database.CreateContext();
            await using var transaction = await context.Database.BeginTransactionAsync();
            var site = new Site { Name = "Demo rollback " + Guid.NewGuid().ToString("N") };
            context.Sites.Add(site);
            await context.SaveChangesAsync();
            var id = site.SiteId;
            await transaction.RollbackAsync();
            context.ChangeTracker.Clear();
            Assert.False(await context.Sites.AnyAsync(row => row.SiteId == id));
        }

        [Theory]
        [InlineData("ApproveDate")]
        [InlineData("ApproveWeek")]
        [InlineData("ApproveMonth")]
        [InlineData("ApproveYear")]
        public async Task OptionalReportDatesTranslateAndExecuteOnSqlServer(string group)
        {
            await using var context = _database.CreateContext();
            var site = new Site { Name = "Demo SQL Report Site" };
            var role = new UserRole { Role = "Demo SQL Role" };
            var employee = new Employee { FName = "Demo", MName = "SQL", LName = "Employee", Position = "Demo", UserRole = role, EmployeeSite = site };
            context.Purchases.Add(new Purchase { RequestedBy = employee, ReceivingSite = site });
            await context.SaveChangesAsync();
            var result = await new ReportService(context, new GeneralReportService(context))
                .GetPurchaseReport(new PurchaseReportDTO { GroupBy = group, SiteId = site.SiteId });
            Assert.NotNull(result.Summary);
            var rows = result.Summary.Cast<object>().ToArray();
            var row = Assert.Single(rows);
            Assert.Equal("N/A", row.GetType().GetProperty("value")!.GetValue(row));
        }
    }

    public sealed class SqlServerDatabase : IAsyncLifetime
    {
        private readonly string _name = "erp_test_" + Guid.NewGuid().ToString("N");
        private string _connection = "";

        public DataContext CreateContext() => new(new DbContextOptionsBuilder<DataContext>()
            .UseSqlServer(_connection).Options);

        public async Task InitializeAsync()
        {
            var server = Environment.GetEnvironmentVariable("ERP_SQL_TEST_CONNECTION");
            if (string.IsNullOrWhiteSpace(server))
                throw new InvalidOperationException("Run make test-sql to supply a disposable SQL Server. No application database is used.");
            // Use the provider's connection string builder so Database/Initial Catalog aliases
            // cannot accidentally redirect schema creation or cleanup to a supplied database.
            using var providerConnection = new Microsoft.Data.SqlClient.SqlConnection();
            var factory = DbProviderFactories.GetFactory(providerConnection)
                ?? throw new InvalidOperationException("The SQL Server provider is unavailable.");
            var connectionBuilder = factory.CreateConnectionStringBuilder()!;
            connectionBuilder.ConnectionString = server;
            connectionBuilder["Initial Catalog"] = _name;
            connectionBuilder["Connect Timeout"] = 5;
            _connection = connectionBuilder.ConnectionString;
            var masterBuilder = factory.CreateConnectionStringBuilder()!;
            masterBuilder.ConnectionString = server;
            masterBuilder["Initial Catalog"] = "master";
            masterBuilder["Connect Timeout"] = 2;
            var ready = false;
            for (var attempt = 0; attempt < 45 && !ready; attempt++)
            {
                await using var master = factory.CreateConnection()!;
                master.ConnectionString = masterBuilder.ConnectionString;
                try { await master.OpenAsync(); ready = true; }
                catch (DbException) { await Task.Delay(TimeSpan.FromSeconds(2)); }
            }
            if (!ready)
            {
                _connection = "";
                throw new InvalidOperationException("The disposable SQL Server did not become ready. Check Docker or ERP_SQL_TEST_CONNECTION; credentials are not logged.");
            }
            try
            {
                await using var context = CreateContext();
                await context.Database.MigrateAsync();
            }
            catch
            {
                await DisposeAsync();
                throw;
            }
        }

        public async Task DisposeAsync()
        {
            if (_connection.Length == 0) return;
            await using var context = CreateContext();
            await context.Database.EnsureDeletedAsync();
        }
    }
}
