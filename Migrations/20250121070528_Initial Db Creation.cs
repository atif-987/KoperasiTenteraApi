using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoperasiTenteraApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialDbCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ICNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileOtp = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    EmailOtp = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    Pin = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    IsMobileVerified = table.Column<bool>(type: "bit", nullable: true),
                    IsEmailVerified = table.Column<bool>(type: "bit", nullable: true),
                    MobileOtpExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmailOtpExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
