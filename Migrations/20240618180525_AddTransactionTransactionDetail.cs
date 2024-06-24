using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BramShop.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionTransactionDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdTransaction",
                table: "TransactionDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdTransaction",
                table: "TransactionDetails");
        }
    }
}
