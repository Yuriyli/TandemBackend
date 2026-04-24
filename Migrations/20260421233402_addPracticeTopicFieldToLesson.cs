using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TandemBackend.Migrations
{
    /// <inheritdoc />
    public partial class addPracticeTopicFieldToLesson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PracticeTopic_LessonId",
                table: "PracticeTopic");

            migrationBuilder.CreateIndex(
                name: "IX_PracticeTopic_LessonId",
                table: "PracticeTopic",
                column: "LessonId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PracticeTopic_LessonId",
                table: "PracticeTopic");

            migrationBuilder.CreateIndex(
                name: "IX_PracticeTopic_LessonId",
                table: "PracticeTopic",
                column: "LessonId");
        }
    }
}
