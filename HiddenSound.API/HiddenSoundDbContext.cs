using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HiddenSound.API.Areas.API.Models;
using HiddenSound.API.Areas.Shared.Models;
using HiddenSound.API.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HiddenSound.API
{
    public class HiddenSoundDbContext : IdentityDbContext<HiddenSoundUser, HiddenSoundRole, int>
    {
        // public virtual DbSet<APIKey> APIKey { get; set; }

        // public virtual DbSet<User> Users { get; set; }

        // public virtual DbSet<Transaction> Transactions { get; set; }

        public HiddenSoundDbContext(DbContextOptions<HiddenSoundDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<HiddenSoundUser>(entity =>
            {
                entity.ToTable(name: "AspNetUser", schema: "Security");
                entity.Property(e => e.Id).HasColumnName("AspNetUserId");

            });

            builder.Entity<HiddenSoundRole>(entity =>
            {
                entity.ToTable(name: "AspNetRole", schema: "Security");
                entity.Property(e => e.Id).HasColumnName("AspNetRoleId");

            });

            builder.Entity<IdentityUserClaim<int>>(entity =>
            {
                entity.ToTable("AspNetUserClaim", "Security");
                entity.Property(e => e.UserId).HasColumnName("AspNetUserId");
                entity.Property(e => e.Id).HasColumnName("AspNetUserClaimId");

            });

            builder.Entity<IdentityUserLogin<int>>(entity =>
            {
                entity.ToTable("AspNetUserLogin", "Security");
                entity.Property(e => e.UserId).HasColumnName("AspNetUserId");

            });

            builder.Entity<IdentityRoleClaim<int>>(entity =>
            {
                entity.ToTable("AspNetRoleClaim", "Security");
                entity.Property(e => e.Id).HasColumnName("AspNetRoleClaimId");
                entity.Property(e => e.RoleId).HasColumnName("AspNetRoleId");
            });

            builder.Entity<IdentityUserRole<int>>(entity =>
            {
                entity.ToTable("AspNetUserRole", "Security");
                entity.Property(e => e.UserId).HasColumnName("AspNetUserId");
                entity.Property(e => e.RoleId).HasColumnName("AspNetRoleId");

            });


            builder.Entity<IdentityUserToken<int>>(entity =>
            {
                entity.ToTable("AspNetUserToken", "Security");
                entity.Property(e => e.UserId).HasColumnName("AspNetUserId");

            });
            //modelBuilder.Entity<APIKey>(entity =>
            //{
            //    entity.Property(e => e.ID).UseSqlServerIdentityColumn().IsRequired();
            //    entity.Property(e => e.PublicKey).IsRequired();
            //    entity.Property(e => e.PrivateKey).IsRequired();
            //});

            //modelBuilder.Entity<User>(entity =>
            //{
            //    entity.Property(e => e.ID).UseSqlServerIdentityColumn().IsRequired();
            //    entity.Property(e => e.Email).IsRequired();
            //    entity.Property(e => e.Password).IsRequired();
            //    entity.Property(e => e.IsVerified).IsRequired();
            //    entity.Property(e => e.IsDeveloper).IsRequired();
            //});

            //modelBuilder.Entity<EmailVerification>(entity =>
            //{

            //});

            //modelBuilder.Entity<Transaction>(entity =>
            //{
            //    entity.Property(e => e.ID).UseSqlServerIdentityColumn();
            //});
        }
    }
}
