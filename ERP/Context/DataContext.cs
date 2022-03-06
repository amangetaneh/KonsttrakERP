using ERP.Models;
using Microsoft.EntityFrameworkCore;

namespace ERP.Context
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemStoreQty>()
                .HasKey(o => new { o.ItemId, o.StoreId });

            modelBuilder.Entity<TransferItem>()
                .HasKey(o => new { o.TransferId, o.ItemId });
            
            modelBuilder.Entity<PurchaseItem>()
                .HasKey(o => new { o.PurchaseId, o.ItemId });
            
            modelBuilder.Entity<ReceiveItem>()
                .HasKey(o => new { o.ReceiveId, o.ItemId });

        }


        public DbSet<ConsumableItem> ConsumableItems { get; set; }

        public DbSet<NonConsumableItem> NonConsumableItems { get; set; }

        //public DbSet<IssuedItem> IssuedItems { get; set; }

        public DbSet<Purchase> Purchases { get; set; }
        
        public DbSet<PurchaseItem> PurchasedItems { get; set; }

        public DbSet<Receive> Receives { get; set; }
        
        public DbSet<ReceiveItem> ReceivedItems { get; set; }

        public DbSet<Transfer> Transfers { get; set; }

        public DbSet<TransferItem> TransferItems { get; set; }
        
        //public DbSet<MaintenanceRequest> MaintenanceRequests { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<UserAccount> UserAccounts { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<Site> Sites { get; set; }

        public DbSet<Store> Stores { get; set; }

        public DbSet<ItemStoreQty> ItemStoreQties { get; set; }
    }
}
