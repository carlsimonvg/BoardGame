using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoardGameAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangeColumnType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoardGameUser_BoardGames_BoardGameId",
                table: "BoardGameUser");

            migrationBuilder.DropForeignKey(
                name: "FK_BoardGameUser_Users_UserId",
                table: "BoardGameUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Plays_BoardGames_BoardGameId",
                table: "Plays");

            migrationBuilder.DropForeignKey(
                name: "FK_Plays_Players_WinnerId",
                table: "Plays");

            migrationBuilder.DropIndex(
                name: "IX_Plays_WinnerId",
                table: "Plays");

            migrationBuilder.DropColumn(
                name: "WinnerId",
                table: "Plays");

            migrationBuilder.AlterColumn<int>(
                name: "BoardGameId",
                table: "Plays",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlayerId",
                table: "Plays",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "BoardGameUser",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BoardGameId",
                table: "BoardGameUser",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Plays_PlayerId",
                table: "Plays",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_BoardGameUser_BoardGames_BoardGameId",
                table: "BoardGameUser",
                column: "BoardGameId",
                principalTable: "BoardGames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BoardGameUser_Users_UserId",
                table: "BoardGameUser",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Plays_BoardGames_BoardGameId",
                table: "Plays",
                column: "BoardGameId",
                principalTable: "BoardGames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Plays_Players_PlayerId",
                table: "Plays",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoardGameUser_BoardGames_BoardGameId",
                table: "BoardGameUser");

            migrationBuilder.DropForeignKey(
                name: "FK_BoardGameUser_Users_UserId",
                table: "BoardGameUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Plays_BoardGames_BoardGameId",
                table: "Plays");

            migrationBuilder.DropForeignKey(
                name: "FK_Plays_Players_PlayerId",
                table: "Plays");

            migrationBuilder.DropIndex(
                name: "IX_Plays_PlayerId",
                table: "Plays");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "Plays");

            migrationBuilder.AlterColumn<int>(
                name: "BoardGameId",
                table: "Plays",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "WinnerId",
                table: "Plays",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "BoardGameUser",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BoardGameId",
                table: "BoardGameUser",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Plays_WinnerId",
                table: "Plays",
                column: "WinnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_BoardGameUser_BoardGames_BoardGameId",
                table: "BoardGameUser",
                column: "BoardGameId",
                principalTable: "BoardGames",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BoardGameUser_Users_UserId",
                table: "BoardGameUser",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Plays_BoardGames_BoardGameId",
                table: "Plays",
                column: "BoardGameId",
                principalTable: "BoardGames",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Plays_Players_WinnerId",
                table: "Plays",
                column: "WinnerId",
                principalTable: "Players",
                principalColumn: "Id");
        }
    }
}
