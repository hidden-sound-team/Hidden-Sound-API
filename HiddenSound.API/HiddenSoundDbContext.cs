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
using OpenIddict.EntityFrameworkCore;
using OpenIddict.Models;
using HiddenSound.API.OpenIddict;

namespace HiddenSound.API
{
    public class HiddenSoundDbContext : IdentityDbContext<HiddenSoundUser, HiddenSoundRole, Guid>
    {
        public virtual DbSet<Authorization> Authorizations { get; set; }

        public virtual DbSet<Device> Devices { get; set; }

        private static readonly string[] SeedRoles = { "Administrator", "Basic" };

        public HiddenSoundDbContext(DbContextOptions<HiddenSoundDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<HiddenSoundUser>(entity =>
            {
                entity.ToTable("User", "Security");
                entity.Property(e => e.Id).HasColumnName("UserId").HasDefaultValueSql("newsequentialid()");

            });

            builder.Entity<HiddenSoundRole>(entity =>
            {
                entity.ToTable("Role", "Security");
                entity.Property(e => e.Id).HasColumnName("RoleId").HasDefaultValueSql("newsequentialid()"); ;

            });

            builder.Entity<IdentityUserClaim<Guid>>(entity =>
            {
                entity.ToTable("UserClaim", "Security");
                entity.Property(e => e.UserId).HasColumnName("UserId");
                entity.Property(e => e.Id).HasColumnName("UserClaimId"); ;

            });

            builder.Entity<IdentityUserLogin<Guid>>(entity =>
            {
                entity.ToTable("UserLogin", "Security");
                entity.Property(e => e.UserId).HasColumnName("UserId");

            });

            builder.Entity<IdentityRoleClaim<Guid>>(entity =>
            {
                entity.ToTable("RoleClaim", "Security");
                entity.Property(e => e.Id).HasColumnName("RoleClaimId"); ;
                entity.Property(e => e.RoleId).HasColumnName("RoleId");
            });

            builder.Entity<IdentityUserRole<Guid>>(entity =>
            {
                entity.ToTable("UserRole", "Security");
                entity.Property(e => e.UserId).HasColumnName("UserId");
                entity.Property(e => e.RoleId).HasColumnName("RoleId");

            });


            builder.Entity<IdentityUserToken<Guid>>(entity =>
            {
                entity.ToTable("UserToken", "Security");
                entity.Property(e => e.UserId).HasColumnName("UserId");

            });

            builder.Entity<HSOpenIddictApplication>(entity =>
            {
                entity.ToTable("OpenIddictApplications");
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
            });

            builder.Entity<HSOpenIddictAuthorization>(entity =>
            {
                entity.ToTable("OpenIddictAuthorizations");
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
            });

            builder.Entity<HSOpenIddictScope>(entity =>
            {
                entity.ToTable("OpenIddictScopes");
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
            });

            builder.Entity<HSOpenIddictToken>(entity =>
            {
                entity.ToTable("OpenIddictTokens");
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
            });

            builder.Entity<Authorization>(entity =>
            {
                entity.ToTable("Authorization");
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
            });

            builder.Entity<Device>(entity =>
            {
                entity.ToTable("Device");
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
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

            foreach (var role in SeedRoles)
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
                    UserName = databaseSeed.AdminUsername,
                    Email = databaseSeed.AdminUsername
                };

                await userManager.CreateAsync(user, databaseSeed.AdminPassword);
            }

            var createdUser = await userManager.FindByNameAsync(databaseSeed.AdminUsername);

            if (!Devices.Any(d => d.UserId == createdUser.Id))
            {
                Devices.Add(new Device()
                {
                    IMEI = "1234",
                    Name = "My Device",
                    UserId = createdUser.Id
                });

                SaveChanges();
            }

            var applicationMananger = serviceProvider.GetRequiredService<OpenIddictApplicationManager<HSOpenIddictApplication>>();

            if (await applicationMananger.FindByClientIdAsync(databaseSeed.ApplicationPublicClientId, cancellationToken) == null)
            {
                var application = new HSOpenIddictApplication
                {
                    ClientId = databaseSeed.ApplicationPublicClientId,
                    DisplayName = "Application Public",
                    RedirectUri = databaseSeed.ApplicationRedirectUri,
                    UserId = createdUser.Id
                };

                await applicationMananger.CreateAsync(application, cancellationToken);
            }

            if (await applicationMananger.FindByClientIdAsync(databaseSeed.ApplicationConfidentialClientId, cancellationToken) == null)
            {
                var application = new HSOpenIddictApplication
                {
                    ClientId = databaseSeed.ApplicationConfidentialClientId,
                    DisplayName = "Application Confidential",
                    RedirectUri = databaseSeed.ApplicationRedirectUri,
                    UserId = createdUser.Id
                };

                await applicationMananger.CreateAsync(application, databaseSeed.ApplicationConfidentialClientId, cancellationToken);
            }
        }
    }
}
