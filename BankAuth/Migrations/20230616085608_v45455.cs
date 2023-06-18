using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankAuth.Migrations
{
    /// <inheritdoc />
    public partial class v45455 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoanId",
                table: "loan_documents");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoanId",
                table: "loan_documents",
                type: "int",
                nullable: true);
        }
    }
}
