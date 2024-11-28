using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IT_Institute_Management.Migrations
{
    /// <inheritdoc />
    public partial class StudentMessageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StudentNIC = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentMessages_Students_StudentNIC",
                        column: x => x.StudentNIC,
                        principalTable: "Students",
                        principalColumn: "NIC",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentMessages_StudentNIC",
                table: "StudentMessages",
                column: "StudentNIC");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentMessages");
        }
    }
}
