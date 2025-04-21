using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyMvcBackend.Migrations
{
    /// <inheritdoc />
    public partial class numques : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AudioUrl",
                table: "ListeningParts");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfQuestions",
                table: "ListeningParts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfQuestions",
                table: "ListeningParts");

            migrationBuilder.AddColumn<string>(
                name: "AudioUrl",
                table: "ListeningParts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
