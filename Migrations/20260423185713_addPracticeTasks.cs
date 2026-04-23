using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TandemBackend.Migrations
{
    /// <inheritdoc />
    public partial class addPracticeTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CodeCompletion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    PracticeTopicId = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeCompletion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeCompletion_PracticeTopic_PracticeTopicId",
                        column: x => x.PracticeTopicId,
                        principalTable: "PracticeTopic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CodeEditor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    PracticeTopicId = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeEditor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeEditor_PracticeTopic_PracticeTopicId",
                        column: x => x.PracticeTopicId,
                        principalTable: "PracticeTopic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Quiz",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    PracticeTopicId = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quiz", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quiz_PracticeTopic_PracticeTopicId",
                        column: x => x.PracticeTopicId,
                        principalTable: "PracticeTopic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CodeCompletionQuestion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CodeCompletionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "TEXT", nullable: false),
                    Hint = table.Column<string>(type: "TEXT", nullable: true),
                    Options = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeCompletionQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeCompletionQuestion_CodeCompletion_CodeCompletionId",
                        column: x => x.CodeCompletionId,
                        principalTable: "CodeCompletion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CodeCompletionRu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CodeCompletionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeCompletionRu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeCompletionRu_CodeCompletion_CodeCompletionId",
                        column: x => x.CodeCompletionId,
                        principalTable: "CodeCompletion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CodeEditorQuestion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CodeEditorId = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Instructions = table.Column<string>(type: "TEXT", nullable: false),
                    StarterCode = table.Column<string>(type: "TEXT", nullable: false),
                    Hint = table.Column<string>(type: "TEXT", nullable: true),
                    ExpectedAnswers = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeEditorQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeEditorQuestion_CodeEditor_CodeEditorId",
                        column: x => x.CodeEditorId,
                        principalTable: "CodeEditor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CodeEditorRu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CodeEditorId = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeEditorRu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeEditorRu_CodeEditor_CodeEditorId",
                        column: x => x.CodeEditorId,
                        principalTable: "CodeEditor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizQuestion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QuizId = table.Column<int>(type: "INTEGER", nullable: false),
                    Question = table.Column<string>(type: "TEXT", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "TEXT", nullable: false),
                    Options = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuizQuestion_Quiz_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quiz",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizRu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QuizId = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizRu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuizRu_Quiz_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quiz",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CodeCompletionQuestionRu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CodeCompletionRuId = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "TEXT", nullable: false),
                    Hint = table.Column<string>(type: "TEXT", nullable: true),
                    Options = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeCompletionQuestionRu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeCompletionQuestionRu_CodeCompletionRu_CodeCompletionRuId",
                        column: x => x.CodeCompletionRuId,
                        principalTable: "CodeCompletionRu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CodeEditorQuestionRu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CodeEditorId = table.Column<int>(type: "INTEGER", nullable: false),
                    CodeEditorRuId = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Instructions = table.Column<string>(type: "TEXT", nullable: false),
                    StarterCode = table.Column<string>(type: "TEXT", nullable: false),
                    Hint = table.Column<string>(type: "TEXT", nullable: true),
                    ExpectedAnswers = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeEditorQuestionRu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeEditorQuestionRu_CodeEditorRu_CodeEditorRuId",
                        column: x => x.CodeEditorRuId,
                        principalTable: "CodeEditorRu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizQuestionRu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QuizRuId = table.Column<int>(type: "INTEGER", nullable: false),
                    Question = table.Column<string>(type: "TEXT", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "TEXT", nullable: false),
                    Options = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizQuestionRu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuizQuestionRu_QuizRu_QuizRuId",
                        column: x => x.QuizRuId,
                        principalTable: "QuizRu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CodeCompletion_PracticeTopicId",
                table: "CodeCompletion",
                column: "PracticeTopicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CodeCompletionQuestion_CodeCompletionId",
                table: "CodeCompletionQuestion",
                column: "CodeCompletionId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeCompletionQuestionRu_CodeCompletionRuId",
                table: "CodeCompletionQuestionRu",
                column: "CodeCompletionRuId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeCompletionRu_CodeCompletionId",
                table: "CodeCompletionRu",
                column: "CodeCompletionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CodeEditor_PracticeTopicId",
                table: "CodeEditor",
                column: "PracticeTopicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CodeEditorQuestion_CodeEditorId",
                table: "CodeEditorQuestion",
                column: "CodeEditorId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeEditorQuestionRu_CodeEditorRuId",
                table: "CodeEditorQuestionRu",
                column: "CodeEditorRuId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeEditorRu_CodeEditorId",
                table: "CodeEditorRu",
                column: "CodeEditorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quiz_PracticeTopicId",
                table: "Quiz",
                column: "PracticeTopicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuizQuestion_QuizId",
                table: "QuizQuestion",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizQuestionRu_QuizRuId",
                table: "QuizQuestionRu",
                column: "QuizRuId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizRu_QuizId",
                table: "QuizRu",
                column: "QuizId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CodeCompletionQuestion");

            migrationBuilder.DropTable(
                name: "CodeCompletionQuestionRu");

            migrationBuilder.DropTable(
                name: "CodeEditorQuestion");

            migrationBuilder.DropTable(
                name: "CodeEditorQuestionRu");

            migrationBuilder.DropTable(
                name: "QuizQuestion");

            migrationBuilder.DropTable(
                name: "QuizQuestionRu");

            migrationBuilder.DropTable(
                name: "CodeCompletionRu");

            migrationBuilder.DropTable(
                name: "CodeEditorRu");

            migrationBuilder.DropTable(
                name: "QuizRu");

            migrationBuilder.DropTable(
                name: "CodeCompletion");

            migrationBuilder.DropTable(
                name: "CodeEditor");

            migrationBuilder.DropTable(
                name: "Quiz");
        }
    }
}
