﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using HiddenSound.API;
using HiddenSound.API.Areas.Shared.Models;

namespace HiddenSound.API.Migrations
{
    [DbContext(typeof(HiddenSoundDbContext))]
    [Migration("20170301204752_latest")]
    partial class latest
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HiddenSound.API.Areas.Shared.Models.Device", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("newsequentialid()");

                    b.Property<string>("IMEI")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<Guid?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Device");
                });

            modelBuilder.Entity("HiddenSound.API.Areas.Shared.Models.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("newsequentialid()");

                    b.Property<string>("AuthorizationCode")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("ExpiresOn");

                    b.Property<int>("Status");

                    b.Property<Guid?>("UserId");

                    b.Property<Guid?>("VendorId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("VendorId");

                    b.ToTable("Transaction");
                });

            modelBuilder.Entity("HiddenSound.API.Identity.HiddenSoundRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("RoleId")
                        .HasDefaultValueSql("newsequentialid()");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("Role","Security");
                });

            modelBuilder.Entity("HiddenSound.API.Identity.HiddenSoundUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("UserId")
                        .HasDefaultValueSql("newsequentialid()");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("User","Security");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("RoleClaimId");

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("RoleId")
                        .HasColumnName("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaim","Security");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("UserClaimId");

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("UserId")
                        .HasColumnName("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaim","Security");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<Guid>("UserId")
                        .HasColumnName("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogin","Security");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnName("UserId");

                    b.Property<Guid>("RoleId")
                        .HasColumnName("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRole","Security");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnName("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserToken","Security");
                });

            modelBuilder.Entity("OpenIddict.Models.OpenIddictApplication<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("newsequentialid()");

                    b.Property<string>("ClientId");

                    b.Property<string>("ClientSecret");

                    b.Property<string>("DisplayName");

                    b.Property<string>("LogoutRedirectUri");

                    b.Property<string>("RedirectUri");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.HasIndex("ClientId")
                        .IsUnique();

                    b.ToTable("OpenIddictApplications");
                });

            modelBuilder.Entity("OpenIddict.Models.OpenIddictAuthorization<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("ApplicationId");

                    b.Property<string>("Scope");

                    b.Property<string>("Subject");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.ToTable("OpenIddictAuthorizations");
                });

            modelBuilder.Entity("OpenIddict.Models.OpenIddictScope<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.HasKey("Id");

                    b.ToTable("OpenIddictScopes");
                });

            modelBuilder.Entity("OpenIddict.Models.OpenIddictToken<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("ApplicationId");

                    b.Property<Guid?>("AuthorizationId");

                    b.Property<string>("Subject");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.HasIndex("AuthorizationId");

                    b.ToTable("OpenIddictTokens");
                });

            modelBuilder.Entity("HiddenSound.API.Areas.Shared.Models.Device", b =>
                {
                    b.HasOne("HiddenSound.API.Identity.HiddenSoundUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("HiddenSound.API.Areas.Shared.Models.Transaction", b =>
                {
                    b.HasOne("HiddenSound.API.Identity.HiddenSoundUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.HasOne("HiddenSound.API.Identity.HiddenSoundUser", "Vendor")
                        .WithMany()
                        .HasForeignKey("VendorId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("HiddenSound.API.Identity.HiddenSoundRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("HiddenSound.API.Identity.HiddenSoundUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("HiddenSound.API.Identity.HiddenSoundUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("HiddenSound.API.Identity.HiddenSoundRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HiddenSound.API.Identity.HiddenSoundUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("OpenIddict.Models.OpenIddictAuthorization<System.Guid>", b =>
                {
                    b.HasOne("OpenIddict.Models.OpenIddictApplication<System.Guid>", "Application")
                        .WithMany("Authorizations")
                        .HasForeignKey("ApplicationId");
                });

            modelBuilder.Entity("OpenIddict.Models.OpenIddictToken<System.Guid>", b =>
                {
                    b.HasOne("OpenIddict.Models.OpenIddictApplication<System.Guid>", "Application")
                        .WithMany("Tokens")
                        .HasForeignKey("ApplicationId");

                    b.HasOne("OpenIddict.Models.OpenIddictAuthorization<System.Guid>", "Authorization")
                        .WithMany("Tokens")
                        .HasForeignKey("AuthorizationId");
                });
        }
    }
}
