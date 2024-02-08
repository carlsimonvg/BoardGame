using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoardGameAPI.Migrations
{
    /// <inheritdoc />
    public partial class MoveColumnsFromBoardGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Owned",
                table: "BoardGames");

            migrationBuilder.DropColumn(
                name: "WishList",
                table: "BoardGames");

            migrationBuilder.AddColumn<bool>(
                name: "Owned",
                table: "BoardGameUser",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "WishList",
                table: "BoardGameUser",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Owned",
                table: "BoardGameUser");

            migrationBuilder.DropColumn(
                name: "WishList",
                table: "BoardGameUser");

            migrationBuilder.AddColumn<bool>(
                name: "Owned",
                table: "BoardGames",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "WishList",
                table: "BoardGames",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
