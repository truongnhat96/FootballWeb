using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballWeb.Migrations
{
    /// <inheritdoc />
    public partial class InitPlayer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Appearances",
                table: "PlayerRecord",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Appearances",
                table: "PlayerRecord");
        }
    }
}
