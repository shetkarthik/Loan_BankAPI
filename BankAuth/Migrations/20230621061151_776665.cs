using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankAuth.Migrations
{
    /// <inheritdoc />
    public partial class _776665 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "ModifiedInterest",
                table: "loan_details",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CIBILScore",
                table: "customer_accountinfo",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModifiedInterest",
                table: "loan_details");

            migrationBuilder.DropColumn(
                name: "CIBILScore",
                table: "customer_accountinfo");
        }
    }
}
