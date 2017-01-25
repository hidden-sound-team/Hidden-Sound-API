using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HiddenSound.API.Areas.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HiddenSound.API
{
    public class HiddenSoundDbContext : DbContext
    {
        public virtual DbSet<APIKey> APIKey { get; set; }

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
        }
    }
}
