using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyMvcBackend.Migrations
{
    /// <inheritdoc />
    public partial class listques : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfQuestions",
                table: "ListeningParts");

            migrationBuilder.AddColumn<string>(
                name: "ListOfQuestions",
                table: "ListeningParts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ListOfQuestions",
                table: "ListeningParts");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfQuestions",
                table: "ListeningParts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
