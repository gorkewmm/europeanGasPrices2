using Core.Entities.Concrete;
using Entities.Concrete;
using Entities.Concrete.Epdk;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security;
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
                //optionsBuilder.UseNpgsql("Host=127.0.0.1;Port=5432;Database=EpdkByCountries;Username=postgres;Password=2021;");
                optionsBuilder.UseNpgsql("Host=127.0.0.1;Port=5432;Database=EpdkByCountries;Username=postgres;Password=2023;");
            }
        }

        public DbSet<FuelPrice> FuelPrices { set; get; }
        public DbSet<OperationClaim> OperationClaims { set; get; }
        public DbSet<User> Users { set; get; }
        public DbSet<UserOperationClaim> UserOperationClaims { set; get; }
        public DbSet<Permission> Permissions { set; get; }
        public DbSet<OperationClaimPermission> OperationClaimPermissions { set; get; }
        public DbSet<EpdkFuelPrice> EpdkFuelPrices { set; get; }

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
            // 1. USER YAPILANDIRMASI
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(u => u.LastName).IsRequired().HasMaxLength(50);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                entity.HasIndex(u => u.Email).IsUnique();

                entity.Property(u => u.NickName).IsRequired().HasMaxLength(50);
                entity.HasIndex(u => u.NickName).IsUnique();

                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.PasswordSalt).IsRequired();
                entity.Property(u => u.Status).HasDefaultValue(true);
            });

            // 2. OPERATION CLAIM (ROL) YAPILANDIRMASI
            modelBuilder.Entity<OperationClaim>(entity =>
            {
                entity.HasKey(oc => oc.Id);
                entity.Property(oc => oc.Name).IsRequired().HasMaxLength(100);
                entity.HasIndex(oc => oc.Name).IsUnique();
            });

            // 3. USER OPERATION CLAIM (ARA TABLO) YAPILANDIRMASI
            modelBuilder.Entity<UserOperationClaim>(entity =>
            {
                entity.HasKey(uoc => uoc.Id);

                entity.HasOne<User>()
                      .WithMany()
                      .HasForeignKey(uoc => uoc.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne<OperationClaim>()
                      .WithMany()
                      .HasForeignKey(uoc => uoc.OperationClaimId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(uoc => new { uoc.UserId, uoc.OperationClaimId }).IsUnique().
                HasFilter("\"IsDeleted\" = false");
            });

            // 4. PERMISSION (YETKİ) YAPILANDIRMASI
            modelBuilder.Entity<Permission>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
                entity.HasIndex(p => p.Name).IsUnique();

                // Description için konfigürasyon
                entity.Property(p => p.Description).HasMaxLength(250);
            });

            // 5. OPERATION CLAIM - PERMISSION (ROL-YETKİ ARA TABLOSU) YAPILANDIRMASI
            modelBuilder.Entity<OperationClaimPermission>(entity =>
            {
                entity.HasKey(ocp => ocp.Id);

                // Role (OperationClaim) ile ilişki
                entity.HasOne<OperationClaim>()
                      .WithMany()
                      .HasForeignKey(ocp => ocp.OperationClaimId)
                      .OnDelete(DeleteBehavior.Cascade); // <-- Senin sorduğun kısım burası!

                // Permission ile ilişki
                entity.HasOne<Permission>()
                      .WithMany()
                      .HasForeignKey(ocp => ocp.PermissionId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(ocp => new { ocp.OperationClaimId, ocp.PermissionId }).IsUnique()
                .HasFilter("\"IsDeleted\" = false");
            });




            //Epdk için ayrı bir tablo oluştur.
            modelBuilder.Entity<EpdkFuelPrice>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Fiyat).IsRequired().
                HasColumnType("numeric(15,5)").IsRequired();

                entity.Property(e => e.Yakit).IsRequired().HasMaxLength(150);

                entity.Property(e => e.Tarih).IsRequired().HasMaxLength(50);

                entity.Property(e => e.OlcuBirimi).IsRequired().HasMaxLength(50);

                entity.Property(x => x.PriceDate).IsRequired();

                entity.HasIndex(e => new { e.Yakit, e.PriceDate }).IsUnique().
                HasFilter("\"IsDeleted\" = false");
            });
        }
    }
}
