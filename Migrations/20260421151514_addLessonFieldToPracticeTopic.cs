using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TandemBackend.Migrations
{
    /// <inheritdoc />
    public partial class addLessonFieldToPracticeTopic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LessonId",
                table: "PracticeTopic",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PracticeTopic_LessonId",
                table: "PracticeTopic",
                column: "LessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_PracticeTopic_Lessons_LessonId",
                table: "PracticeTopic",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PracticeTopic_Lessons_LessonId",
                table: "PracticeTopic");

            migrationBuilder.DropIndex(
                name: "IX_PracticeTopic_LessonId",
                table: "PracticeTopic");

            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "PracticeTopic");
        }
    }
}
