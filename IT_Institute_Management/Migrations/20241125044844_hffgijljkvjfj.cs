using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IT_Institute_Management.Migrations
{
    /// <inheritdoc />
    public partial class hffgijljkvjfj : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresss_Students_StudentNIC",
                table: "Addresss");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Addresss",
                table: "Addresss");

            migrationBuilder.RenameTable(
                name: "Addresss",
                newName: "Address");

            migrationBuilder.RenameIndex(
                name: "IX_Addresss_StudentNIC",
                table: "Address",
                newName: "IX_Address_StudentNIC");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Address",
                table: "Address",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_Students_StudentNIC",
                table: "Address",
                column: "StudentNIC",
                principalTable: "Students",
                principalColumn: "NIC");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_Students_StudentNIC",
                table: "Address");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Address",
                table: "Address");

            migrationBuilder.RenameTable(
                name: "Address",
                newName: "Addresss");

            migrationBuilder.RenameIndex(
                name: "IX_Address_StudentNIC",
                table: "Addresss",
                newName: "IX_Addresss_StudentNIC");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Addresss",
                table: "Addresss",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresss_Students_StudentNIC",
                table: "Addresss",
                column: "StudentNIC",
                principalTable: "Students",
                principalColumn: "NIC");
        }
    }
}
