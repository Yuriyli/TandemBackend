using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TandemBackend.Migrations
{
    /// <inheritdoc />
    public partial class fixWrongNameField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodeEditorId",
                table: "CodeEditorQuestionRu");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CodeEditorId",
                table: "CodeEditorQuestionRu",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
