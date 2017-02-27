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
    public class HiddenSoundDbContext : IdentityDbContext<HiddenSoundUser, HiddenSoundRole, Guid>
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
                entity.Property(e => e.Id).HasColumnName("AspNetUserId").HasDefaultValueSql("newsequentialid()");

            });

            builder.Entity<HiddenSoundRole>(entity =>
            {
                entity.ToTable(name: "AspNetRole", schema: "Security");
                entity.Property(e => e.Id).HasColumnName("AspNetRoleId").HasDefaultValueSql("newsequentialid()"); ;

            });

            builder.Entity<IdentityUserClaim<Guid>>(entity =>
            {
                entity.ToTable("AspNetUserClaim", "Security");
                entity.Property(e => e.UserId).HasColumnName("AspNetUserId");
                entity.Property(e => e.Id).HasColumnName("AspNetUserClaimId"); ;

            });

            builder.Entity<IdentityUserLogin<Guid>>(entity =>
            {
                entity.ToTable("AspNetUserLogin", "Security");
                entity.Property(e => e.UserId).HasColumnName("AspNetUserId");

            });

            builder.Entity<IdentityRoleClaim<Guid>>(entity =>
            {
                entity.ToTable("AspNetRoleClaim", "Security");
                entity.Property(e => e.Id).HasColumnName("AspNetRoleClaimId"); ;
                entity.Property(e => e.RoleId).HasColumnName("AspNetRoleId");
            });

            builder.Entity<IdentityUserRole<Guid>>(entity =>
            {
                entity.ToTable("AspNetUserRole", "Security");
                entity.Property(e => e.UserId).HasColumnName("AspNetUserId");
                entity.Property(e => e.RoleId).HasColumnName("AspNetRoleId");

            });


            builder.Entity<IdentityUserToken<Guid>>(entity =>
            {
                entity.ToTable("AspNetUserToken", "Security");
                entity.Property(e => e.UserId).HasColumnName("AspNetUserId");

            });

            builder.Entity<OpenIddictApplication<Guid>>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
            });

            builder.Entity<OpenIddictApplication<Guid>>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
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

            var databaseSeed = serviceProvider.GetRequiredService<IOptions<DatabaseSeedConfig>>().Value;

            var roleManager = serviceProvider.GetRequiredService<RoleManager<HiddenSoundRole>>();

            foreach (var role in Roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new HiddenSoundRole(role));
                }
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<HiddenSoundUser>>();

            if (await userManager.FindByNameAsync(databaseSeed.AdminUsername) == null)
            {
                var user = new HiddenSoundUser
                {
                    EmailConfirmed = true,
                    UserName = databaseSeed.AdminUsername
                };

                await userManager.CreateAsync(user, databaseSeed.AdminPassword);
            }

            var applicationMananger = serviceProvider.GetRequiredService<OpenIddictApplicationManager<OpenIddictApplication<Guid>>>();

            if (await applicationMananger.FindByClientIdAsync(databaseSeed.ApplicationPublicClientId, cancellationToken) == null)
            {
                var application = new OpenIddictApplication<Guid>
                {
                    ClientId = databaseSeed.ApplicationPublicClientId,
                    DisplayName = "Application Public",
                    RedirectUri = databaseSeed.ApplicationRedirectUri
                };

                await applicationMananger.CreateAsync(application, cancellationToken);
            }

            if (await applicationMananger.FindByClientIdAsync(databaseSeed.ApplicationConfidentialClientId, cancellationToken) == null)
            {
                var application = new OpenIddictApplication<Guid>
                {
                    ClientId = databaseSeed.ApplicationConfidentialClientId,
                    DisplayName = "Application Confidential",
                    RedirectUri = databaseSeed.ApplicationRedirectUri
                };

                await applicationMananger.CreateAsync(application, databaseSeed.ApplicationConfidentialClientId, cancellationToken);
            }
        }
    }
}
