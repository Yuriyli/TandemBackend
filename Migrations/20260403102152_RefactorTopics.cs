using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TandemBackend.Migrations
{
    /// <inheritdoc />
    public partial class RefactorTopics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Topics",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Topics");

            migrationBuilder.RenameTable(
                name: "Topics",
                newName: "Topic");

            migrationBuilder.RenameColumn(
                name: "TitleRu",
                table: "Topic",
                newName: "Example");

            migrationBuilder.RenameColumn(
                name: "ContentRu",
                table: "Topic",
                newName: "Description");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Topic",
                table: "Topic",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Topic",
                table: "Topic");

            migrationBuilder.RenameTable(
                name: "Topic",
                newName: "Topics");

            migrationBuilder.RenameColumn(
                name: "Example",
                table: "Topics",
                newName: "TitleRu");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Topics",
                newName: "ContentRu");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Topics",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Topics",
                table: "Topics",
                column: "Id");
        }
    }
}
