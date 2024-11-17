using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IT_Institute_Management.Migrations
{
    /// <inheritdoc />
    public partial class efw3tj4jes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DueAmount",
                table: "Payments",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "FullAmount",
                table: "Payments",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DueAmount",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "FullAmount",
                table: "Payments");
        }
    }
}
