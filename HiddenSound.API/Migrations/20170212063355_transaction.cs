using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HiddenSound.API.Migrations
{
    public partial class transaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Authorization_Code = table.Column<string>(maxLength: 50, nullable: false),
                    Expires_On = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    User_ID = table.Column<int>(nullable: true),
                    Vendor_ID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Transaction_AspNetUser_User_ID",
                        column: x => x.User_ID,
                        principalSchema: "Security",
                        principalTable: "AspNetUser",
                        principalColumn: "AspNetUserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transaction_AspNetUser_Vendor_ID",
                        column: x => x.Vendor_ID,
                        principalSchema: "Security",
                        principalTable: "AspNetUser",
                        principalColumn: "AspNetUserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_User_ID",
                table: "Transaction",
                column: "User_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_Vendor_ID",
                table: "Transaction",
                column: "Vendor_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transaction");
        }
    }
}
