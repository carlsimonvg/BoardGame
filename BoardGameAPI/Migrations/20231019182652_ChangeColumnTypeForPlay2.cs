using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoardGameAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangeColumnTypeForPlay2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.DropColumn(
		        name: "TimePlayed",
		        table: "Plays");

	        migrationBuilder.AddColumn<TimeSpan>(
		        name: "TimePlayed",
		        table: "Plays",
		        type: "time",
		        nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.DropColumn(
		        name: "TimePlayed",
		        table: "Plays");

	        migrationBuilder.AddColumn<int>(
		        name: "TimePlayed",
		        table: "Plays",
		        type: "int",
		        nullable: false);
		}
    }
}
