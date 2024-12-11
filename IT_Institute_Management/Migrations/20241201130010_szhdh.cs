using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IT_Institute_Management.Migrations
{
    /// <inheritdoc />
    public partial class szhdh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocialMediaLinks_Students_StudentNIC",
                table: "SocialMediaLinks");

            migrationBuilder.AddForeignKey(
                name: "FK_SocialMediaLinks_Students_StudentNIC",
                table: "SocialMediaLinks",
                column: "StudentNIC",
                principalTable: "Students",
                principalColumn: "NIC",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocialMediaLinks_Students_StudentNIC",
                table: "SocialMediaLinks");

            migrationBuilder.AddForeignKey(
                name: "FK_SocialMediaLinks_Students_StudentNIC",
                table: "SocialMediaLinks",
                column: "StudentNIC",
                principalTable: "Students",
                principalColumn: "NIC");
        }
    }
}
