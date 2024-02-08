using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoardGameAPI.Migrations
{
    /// <inheritdoc />
    public partial class LinkUserAndBoardgame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BoardGameUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoardGameId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardGameUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BoardGameUser_BoardGames_BoardGameId",
                        column: x => x.BoardGameId,
                        principalTable: "BoardGames",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BoardGameUser_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoardGameUser_BoardGameId",
                table: "BoardGameUser",
                column: "BoardGameId");

            migrationBuilder.CreateIndex(
                name: "IX_BoardGameUser_UserId",
                table: "BoardGameUser",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoardGameUser");
        }
    }
}
