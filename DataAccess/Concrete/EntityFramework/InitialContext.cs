using Core.Entities.Concrete;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    public class InitialContext : DbContext
    {
        // Mevcut constructor'ın hemen üstüne veya altına ekleyin:
        public InitialContext()
        {
        }

        // EfEntityRepositoryBase'in new() yapabilmesi için bağlantı ayarını burada da yapmanız gerekebilir:
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // ÖRNEK: Initial Catalog=EpdkByCountries; kısmını ekledik. Kendi DB adınızı yazın.
                optionsBuilder.UseSqlServer("Data Source=HALILGORKEM;Initial Catalog=EpdkByCountries;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Application Name=\"SQL Server Management Studio\";Command Timeout=0");
            }
        }


        public DbSet<FuelPrice> FuelPrices { get; set; }

        public override int SaveChanges()
        {
            SetAuditProperties();
            return base.SaveChanges();
        }

        //public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        //{
        //    SetAuditProperties();
        //    return await base.SaveChangesAsync(cancellationToken);
        //}

        private void SetAuditProperties()
        {
            // BaseEntity'den türeyen tüm hareketleri izle
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now;
                        entry.Entity.IsDeleted = false;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedDate = DateTime.Now;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified; // Silme işlemini iptal et ve güncelleme olarak işaretle
                        entry.Entity.IsDeleted = true;
                        //entry.Entity.UpdatedDate = DateTime.Now;
                        entry.Entity.DeletedDate = DateTime.Now;
                        break;

                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FuelPrice>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Country) 
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(x => x.Currency)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(x => x.Gasoline)
                      .HasColumnType("decimal(10,3)");

                entity.Property(x => x.Diesel)
                      .HasColumnType("decimal(10,3)");

                entity.Property(x => x.Lpg)
                      .HasColumnType("decimal(10,3)");

                entity.Property(x => x.PriceDate)
                      .IsRequired();

                entity.HasIndex(x => new { x.Country, x.PriceDate })
                      .IsUnique();
            });
        }
    }
}
