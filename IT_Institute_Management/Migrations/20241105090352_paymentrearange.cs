using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IT_Institute_Management.Migrations
{
    /// <inheritdoc />
    public partial class paymentrearange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_Students_StudentNIC",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Enrollment_EnrollmentId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Address_StudentNIC",
                table: "Address");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payment",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "NotificationId",
                table: "Students");

            migrationBuilder.RenameTable(
                name: "Payment",
                newName: "Payments");

            migrationBuilder.RenameColumn(
                name: "Province",
                table: "Address",
                newName: "ZipCode");

            migrationBuilder.RenameColumn(
                name: "District",
                table: "Address",
                newName: "State");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_EnrollmentId",
                table: "Payments",
                newName: "IX_Payments_EnrollmentId");

            migrationBuilder.AddColumn<string>(
                name: "WhatsappNuber",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "StudentNIC",
                table: "Address",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Address",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Address",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payments",
                table: "Payments",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Address_StudentNIC",
                table: "Address",
                column: "StudentNIC",
                unique: true,
                filter: "[StudentNIC] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_Students_StudentNIC",
                table: "Address",
                column: "StudentNIC",
                principalTable: "Students",
                principalColumn: "NIC");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Enrollment_EnrollmentId",
                table: "Payments",
                column: "EnrollmentId",
                principalTable: "Enrollment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_Students_StudentNIC",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Enrollment_EnrollmentId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Address_StudentNIC",
                table: "Address");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payments",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "WhatsappNuber",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Address");

            migrationBuilder.RenameTable(
                name: "Payments",
                newName: "Payment");

            migrationBuilder.RenameColumn(
                name: "ZipCode",
                table: "Address",
                newName: "Province");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "Address",
                newName: "District");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_EnrollmentId",
                table: "Payment",
                newName: "IX_Payment_EnrollmentId");

            migrationBuilder.AddColumn<Guid>(
                name: "NotificationId",
                table: "Students",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "StudentNIC",
                table: "Address",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payment",
                table: "Payment",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Address_StudentNIC",
                table: "Address",
                column: "StudentNIC",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Address_Students_StudentNIC",
                table: "Address",
                column: "StudentNIC",
                principalTable: "Students",
                principalColumn: "NIC",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Enrollment_EnrollmentId",
                table: "Payment",
                column: "EnrollmentId",
                principalTable: "Enrollment",
                principalColumn: "Id");
        }
    }
}
