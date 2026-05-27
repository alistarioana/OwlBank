using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OwlBank.Migrations
{
    /// <inheritdoc />
    public partial class AddBlockedTagForCards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBlocked",
                table: "Cards",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBlocked",
                table: "Cards");
        }
    }
}
