using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankAuth.Migrations
{
    /// <inheritdoc />
    public partial class v34234 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoanComment",
                table: "loan_details");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LoanComment",
                table: "loan_details",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
