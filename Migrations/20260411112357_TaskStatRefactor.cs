using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TandemBackend.Migrations
{
    /// <inheritdoc />
    public partial class TaskStatRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaskId",
                table: "TaskStats",
                newName: "WrongAnswers");

            migrationBuilder.RenameColumn(
                name: "IsFinished",
                table: "TaskStats",
                newName: "EarnedPoints");

            migrationBuilder.AddColumn<int>(
                name: "CorrectAnswers",
                table: "TaskStats",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Difficulty",
                table: "TaskStats",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LessonName",
                table: "TaskStats",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectAnswers",
                table: "TaskStats");

            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "TaskStats");

            migrationBuilder.DropColumn(
                name: "LessonName",
                table: "TaskStats");

            migrationBuilder.RenameColumn(
                name: "WrongAnswers",
                table: "TaskStats",
                newName: "TaskId");

            migrationBuilder.RenameColumn(
                name: "EarnedPoints",
                table: "TaskStats",
                newName: "IsFinished");
        }
    }
}
