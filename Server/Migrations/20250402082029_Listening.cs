using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyMvcBackend.Migrations
{
    /// <inheritdoc />
    public partial class Listening : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListeningQuestions_ListeningTests_ListeningTestId",
                table: "ListeningQuestions");

            migrationBuilder.DropColumn(
                name: "AudioUrl",
                table: "ListeningTests");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ListeningTests");

            migrationBuilder.DropColumn(
                name: "AnswerType",
                table: "ListeningQuestions");

            migrationBuilder.DropColumn(
                name: "CorrectAnswer",
                table: "ListeningQuestions");

            migrationBuilder.RenameColumn(
                name: "ListeningTestId",
                table: "ListeningQuestions",
                newName: "PartId");

            migrationBuilder.RenameIndex(
                name: "IX_ListeningQuestions_ListeningTestId",
                table: "ListeningQuestions",
                newName: "IX_ListeningQuestions_PartId");

            migrationBuilder.AlterColumn<string>(
                name: "QuestionText",
                table: "ListeningQuestions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "ListeningAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnswerText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListeningAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListeningAnswers_ListeningQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "ListeningQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListeningRecordings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AudioUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListeningRecordings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListeningRecordings_ListeningTests_TestId",
                        column: x => x.TestId,
                        principalTable: "ListeningTests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListeningParts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AudioUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecordingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListeningParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListeningParts_ListeningRecordings_RecordingId",
                        column: x => x.RecordingId,
                        principalTable: "ListeningRecordings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListeningAnswers_QuestionId",
                table: "ListeningAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_ListeningParts_RecordingId",
                table: "ListeningParts",
                column: "RecordingId");

            migrationBuilder.CreateIndex(
                name: "IX_ListeningRecordings_TestId",
                table: "ListeningRecordings",
                column: "TestId");

            migrationBuilder.AddForeignKey(
                name: "FK_ListeningQuestions_ListeningParts_PartId",
                table: "ListeningQuestions",
                column: "PartId",
                principalTable: "ListeningParts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListeningQuestions_ListeningParts_PartId",
                table: "ListeningQuestions");

            migrationBuilder.DropTable(
                name: "ListeningAnswers");

            migrationBuilder.DropTable(
                name: "ListeningParts");

            migrationBuilder.DropTable(
                name: "ListeningRecordings");

            migrationBuilder.RenameColumn(
                name: "PartId",
                table: "ListeningQuestions",
                newName: "ListeningTestId");

            migrationBuilder.RenameIndex(
                name: "IX_ListeningQuestions_PartId",
                table: "ListeningQuestions",
                newName: "IX_ListeningQuestions_ListeningTestId");

            migrationBuilder.AddColumn<string>(
                name: "AudioUrl",
                table: "ListeningTests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ListeningTests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "QuestionText",
                table: "ListeningQuestions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AnswerType",
                table: "ListeningQuestions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CorrectAnswer",
                table: "ListeningQuestions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_ListeningQuestions_ListeningTests_ListeningTestId",
                table: "ListeningQuestions",
                column: "ListeningTestId",
                principalTable: "ListeningTests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
