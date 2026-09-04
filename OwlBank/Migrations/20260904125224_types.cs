using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OwlBank.Migrations
{
    /// <inheritdoc />
    public partial class types : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Users_UserId",
                table: "Transaction");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Transaction",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "Details",
                table: "Transaction",
                newName: "Description");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_UserId",
                table: "Transaction",
                newName: "IX_Transaction_UserID");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Transaction",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Transaction",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "BankStatement",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Users_UserID",
                table: "Transaction",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Users_UserID",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "BankStatement");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Transaction",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Transaction",
                newName: "Details");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_UserID",
                table: "Transaction",
                newName: "IX_Transaction_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Users_UserId",
                table: "Transaction",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "ID");
        }
    }
}
