using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IT_Institute_Management.Migrations
{
    /// <inheritdoc />
    public partial class AddSocialMediaLinksTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocialMediaLinks_Students_StudentNIC",
                table: "SocialMediaLinks");

            migrationBuilder.DropIndex(
                name: "IX_SocialMediaLinks_StudentNIC",
                table: "SocialMediaLinks");

            migrationBuilder.AlterColumn<string>(
                name: "WhatsApp",
                table: "SocialMediaLinks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "StudentNIC",
                table: "SocialMediaLinks",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LinkedIn",
                table: "SocialMediaLinks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Instagram",
                table: "SocialMediaLinks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "GitHub",
                table: "SocialMediaLinks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Facebook",
                table: "SocialMediaLinks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_SocialMediaLinks_StudentNIC",
                table: "SocialMediaLinks",
                column: "StudentNIC",
                unique: true,
                filter: "[StudentNIC] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_SocialMediaLinks_Students_StudentNIC",
                table: "SocialMediaLinks",
                column: "StudentNIC",
                principalTable: "Students",
                principalColumn: "NIC");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocialMediaLinks_Students_StudentNIC",
                table: "SocialMediaLinks");

            migrationBuilder.DropIndex(
                name: "IX_SocialMediaLinks_StudentNIC",
                table: "SocialMediaLinks");

            migrationBuilder.AlterColumn<string>(
                name: "WhatsApp",
                table: "SocialMediaLinks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StudentNIC",
                table: "SocialMediaLinks",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LinkedIn",
                table: "SocialMediaLinks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Instagram",
                table: "SocialMediaLinks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GitHub",
                table: "SocialMediaLinks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Facebook",
                table: "SocialMediaLinks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SocialMediaLinks_StudentNIC",
                table: "SocialMediaLinks",
                column: "StudentNIC",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SocialMediaLinks_Students_StudentNIC",
                table: "SocialMediaLinks",
                column: "StudentNIC",
                principalTable: "Students",
                principalColumn: "NIC",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
