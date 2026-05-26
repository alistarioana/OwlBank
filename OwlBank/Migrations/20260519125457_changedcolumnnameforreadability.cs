using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OwlBank.Migrations
{
    /// <inheritdoc />
    public partial class changedcolumnnameforreadability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LockedAccount",
                table: "Users",
                newName: "AccountLocketAt");

            migrationBuilder.RenameColumn(
                name: "FailedLogin",
                table: "Users",
                newName: "LoginAttempt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LoginAttempt",
                table: "Users",
                newName: "FailedLogin");

            migrationBuilder.RenameColumn(
                name: "AccountLocketAt",
                table: "Users",
                newName: "LockedAccount");
        }
    }
}
