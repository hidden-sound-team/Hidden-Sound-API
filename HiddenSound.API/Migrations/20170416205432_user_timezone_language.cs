using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HiddenSound.API.Migrations
{
    public partial class user_timezone_language : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Language",
                schema: "Security",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Timezone",
                schema: "Security",
                table: "User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Language",
                schema: "Security",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Timezone",
                schema: "Security",
                table: "User");
        }
    }
}
