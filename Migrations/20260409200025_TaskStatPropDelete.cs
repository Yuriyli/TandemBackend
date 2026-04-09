using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TandemBackend.Migrations
{
    /// <inheritdoc />
    public partial class TaskStatPropDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaskDifficulty",
                table: "TaskStats");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TaskDifficulty",
                table: "TaskStats",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
