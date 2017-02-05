using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HiddenSound.API.Areas.API.Models;
using HiddenSound.API.Areas.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace HiddenSound.API
{
    public class HiddenSoundDbContext : DbContext
    {
        public virtual DbSet<APIKey> APIKey { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Transaction> Transactions { get; set; }

        public HiddenSoundDbContext(DbContextOptions<HiddenSoundDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<APIKey>(entity =>
            {
                entity.Property(e => e.ID).UseSqlServerIdentityColumn().IsRequired();
                entity.Property(e => e.PublicKey).IsRequired();
                entity.Property(e => e.PrivateKey).IsRequired();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.ID).UseSqlServerIdentityColumn().IsRequired();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.Password).IsRequired();
                entity.Property(e => e.IsVerified).IsRequired();
                entity.Property(e => e.IsDeveloper).IsRequired();
            });

            modelBuilder.Entity<EmailVerification>(entity =>
            {
                
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.ID).UseSqlServerIdentityColumn().IsRequired();
                entity.Property(e => e.AuthorizationCode).IsRequired();
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.Base64QR).IsRequired();
                entity.Property(e => e.UserID).IsRequired();
                entity.Property(e => e.VendorID).IsRequired();
                entity.Property(e => e.ExpiresOn).IsRequired();
            });
        }
    }
}
