using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using HiddenSound.API.Areas.API.Models;
using HiddenSound.API.Areas.Shared.Models;
using HiddenSound.API.Configuration;
using HiddenSound.API.Helpers;
using HiddenSound.API.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenIddict.Core;
using OpenIddict.Models;

namespace HiddenSound.API
{
    public class HiddenSoundDbContext : IdentityDbContext<HiddenSoundUser, HiddenSoundRole, int>
    {
        public virtual DbSet<Transaction> Transactions { get; set; }

        private static readonly string[] Roles = { "Administrator", "Basic" };

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

            builder.Entity<Transaction>(entity =>
            {

            });

            builder.Entity<Device>(entity =>
            {

            });
        }

        public async Task EnsureSeedData(IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            if (Database.GetPendingMigrations().Any())
            {
                await Database.MigrateAsync(cancellationToken);
            }

            var roleManager = serviceProvider.GetRequiredService<RoleManager<HiddenSoundRole>>();

            foreach (var role in Roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new HiddenSoundRole(role));
                }
            }

            var applicationMananger = serviceProvider.GetRequiredService<OpenIddictApplicationManager<OpenIddictApplication<int>>>();
            var appSettings = serviceProvider.GetRequiredService<IOptions<AppSettingsConfig>>().Value;

            if (await applicationMananger.FindByClientIdAsync(appSettings.ClientId, cancellationToken) == null)
            {
                var application = new OpenIddictApplication<int>
                {
                    ClientId = appSettings.ClientId,
                    DisplayName = "Application"
                };

                await applicationMananger.CreateAsync(application, cancellationToken);
            }
        }
    }
}
