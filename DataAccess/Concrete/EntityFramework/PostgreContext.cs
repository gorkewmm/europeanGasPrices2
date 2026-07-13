using Core.Entities.Concrete;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    public class PostgreContext : DbContext
    {
        public PostgreContext()
        {
            // PostgreSQL'in eski tip DateTime davranışını (MSSQL gibi davranmasını) sağlar.
            // Bu satır olmazsa DateTime.Now yazdığın yerlerde sistem hata verir.
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Sabit bağlantı adresi (Fallback)
                //optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=EpdkByCountries;Username=postgres;Password=4321;");
                optionsBuilder.UseNpgsql("Host=127.0.0.1;Port=5432;Database=EpdkByCountries;Username=postgres;Password=2021;");
            }
        }

        public DbSet<FuelPrice> FuelPrices { set; get; }

        public override int SaveChanges()
        {
            SetAuditProperties();
            return base.SaveChanges();
        }

        private void SetAuditProperties()
        {
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
                        entry.State = EntityState.Modified;
                        entry.Entity.IsDeleted = true;
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

                // PostgreSQL için decimal(10,3) karşılığı
                entity.Property(x => x.Gasoline)
                      .HasColumnType("numeric(10,3)");

                entity.Property(x => x.Diesel)
                      .HasColumnType("numeric(10,3)");

                entity.Property(x => x.Lpg)
                      .HasColumnType("numeric(10,3)");

                entity.Property(x => x.PriceDate)
                      .IsRequired();

                entity.HasIndex(x => new { x.Country, x.PriceDate })
                      .IsUnique();
            });
        }
    }
}
