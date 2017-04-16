using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HiddenSound.API.Migrations
{
    public partial class app_desc_url : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "OpenIddictApplications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WebsiteUri",
                table: "OpenIddictApplications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "OpenIddictApplications");

            migrationBuilder.DropColumn(
                name: "WebsiteUri",
                table: "OpenIddictApplications");
        }
    }
}
