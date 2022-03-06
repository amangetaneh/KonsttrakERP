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


        public DbSet<ConsumableItem> ConsumableItems { get; set; }

        public DbSet<NonConsumableItem> NonConsumableItems { get; set; }

        public DbSet<Issue> Issues { get; set; }

        public DbSet<IssueItem> IssueItems { get; set; }

        public DbSet<Maintenance> Maintenances { get; set; }

        public DbSet<Purchase> Purchases { get; set; }
        
        public DbSet<PurchaseItem> PurchasedItems { get; set; }

        public DbSet<Receive> Receives { get; set; }
        
        public DbSet<ReceiveItem> ReceivedItems { get; set; }

        public DbSet<Transfer> Transfers { get; set; }

        public DbSet<TransferItem> TransferItems { get; set; }
      
        public DbSet<Employee> Employees { get; set; }

        public DbSet<UserAccount> UserAccounts { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<Site> Sites { get; set; }

        public DbSet<Store> Stores { get; set; }

        public DbSet<ItemStoreQty> ItemStoreQties { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Set Composite Foreign keys
            modelBuilder.Entity<ItemStoreQty>()
                .HasKey(o => new { o.ItemId, o.StoreId });

            modelBuilder.Entity<TransferItem>()
                .HasKey(o => new { o.TransferId, o.ItemId });

            modelBuilder.Entity<IssueItem>()
                .HasKey(o => new { o.IssueId, o.ItemId });

            modelBuilder.Entity<PurchaseItem>()
                .HasKey(o => new { o.PurchaseId, o.ItemId });

            modelBuilder.Entity<ReceiveItem>()
                .HasKey(o => new { o.ReceiveId, o.ItemId });

            //Transfer
            modelBuilder.Entity<Transfer>()
                    .HasOne(t => t.ReceiveStore)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transfer>()
                    .HasOne(t => t.SendStore)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transfer>()
                    .HasOne(t => t.RequestedBy)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transfer>()
                    .HasOne(t => t.ReceivedBy)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transfer>()
                    .HasOne(t => t.DeliveredBy)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transfer>()
                    .HasOne(t => t.ApprovedBy)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

            //Maintenance
            modelBuilder.Entity<Maintenance>()
                   .HasOne(t => t.RequestedBy)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Maintenance>()
                   .HasOne(t => t.ApprovedBy)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict);

            //Issue
            modelBuilder.Entity<Issue>()
                    .HasOne(i => i.HandedBy)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Issue>()
                    .HasOne(i => i.ReceivedBy)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Issue>()
                    .HasOne(i => i.ApprovedBy)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

            //Purchase
            modelBuilder.Entity<Purchase>()
                    .HasOne(i => i.RequestStore)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Purchase>()
                   .HasOne(i => i.RequestedBy)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Purchase>()
                   .HasOne(i => i.CheckedBy)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Purchase>()
                   .HasOne(i => i.ApprovedBy)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict);

            //Receive
            modelBuilder.Entity<Receive>()
                   .HasOne(i => i.ReceiveStore)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Receive>()
                   .HasOne(i => i.Purchase)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Receive>()
                   .HasOne(i => i.DeliveredBy)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Receive>()
                   .HasOne(i => i.ReceivedBy)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Receive>()
                   .HasOne(i => i.ApprovedBy)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
