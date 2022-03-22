using ERP.Models;
using Microsoft.EntityFrameworkCore;

namespace ERP.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {

        }


        public DbSet<Material> Materials { get; set; }

        public DbSet<Equipment> Equipments { get; set; }

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

        public DbSet<ItemSiteQty> ItemSiteQties { get; set; }

        public DbSet<EquipmentCategory> EquipmentCategories { get; set; }

        public DbSet<EquipmentModel> EquipmentModels { get; set; }

        public DbSet<EquipmentAsset> EquipmentAssets { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            //Set Composite keys
            modelBuilder.Entity<ItemSiteQty>()
                .HasKey(o => new { o.ItemId, o.SiteId });

            modelBuilder.Entity<TransferItem>()
                .HasKey(o => new { o.TransferId, o.ItemId, o.EquipmentModelId });

            modelBuilder.Entity<TransferItemEquipmentAsset>()
                .HasKey(o => new { o.TransferId, o.ItemId, o.EquipmentModelId, o.EquipmentAssetId });

            modelBuilder.Entity<IssueItem>()
                .HasKey(o => new { o.IssueId, o.ItemId });

            modelBuilder.Entity<PurchaseItem>()
                .HasKey(o => new { o.PurchaseId, o.ItemId });

            modelBuilder.Entity<ReceiveItem>()
                .HasKey(o => new { o.ReceiveId, o.ItemId });

            //Transfer
            modelBuilder.Entity<Transfer>()
                    .HasOne(t => t.ReceiveSite)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transfer>()
                    .HasOne(t => t.SendSite)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transfer>()
                    .HasOne(t => t.RequestedBy)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transfer>()
                    .HasOne(t => t.ReceivedBy)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);

            modelBuilder.Entity<Transfer>()
                    .HasOne(t => t.DeliveredBy)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);

            modelBuilder.Entity<Transfer>()
                    .HasOne(t => t.ApprovedBy)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);

            modelBuilder.Entity<Transfer>()
                    .HasOne(t => t.SentBy)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);



            //Maintenance
            modelBuilder.Entity<Maintenance>()
                   .HasOne(t => t.RequestedBy)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Maintenance>()
                   .HasOne(t => t.ApprovedBy)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired(false);

            modelBuilder.Entity<Maintenance>()
                   .HasOne(t => t.FixedBy)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired(false);

            //Issue
            modelBuilder.Entity<Issue>()
                    .HasOne(i => i.HandedBy)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);

            modelBuilder.Entity<Issue>()
                    .HasOne(i => i.RequestedBy)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Issue>()
                    .HasOne(i => i.ApprovedBy)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);

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
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired(false);

            modelBuilder.Entity<Purchase>()
                   .HasOne(i => i.ApprovedBy)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired(false);

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
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired(false);

            //TransferItem-EquipmentAsset
            modelBuilder.Entity<TransferItemEquipmentAsset>()
                .HasOne(o => o.TransferItem)
                .WithMany(o => o.TransferEquipmentAssets)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
