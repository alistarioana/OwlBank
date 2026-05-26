using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OwlBank.Migrations
{
    /// <inheritdoc />
    public partial class AddFailedLoginAndLockedAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankStatement_Users_UserId",
                table: "BankStatement");

            migrationBuilder.AddColumn<int>(
                name: "FailedLogin",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LockedAccount",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "BankStatement",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_BankStatement_Users_UserId",
                table: "BankStatement",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankStatement_Users_UserId",
                table: "BankStatement");

            migrationBuilder.DropColumn(
                name: "FailedLogin",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LockedAccount",
                table: "Users");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "BankStatement",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BankStatement_Users_UserId",
                table: "BankStatement",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
