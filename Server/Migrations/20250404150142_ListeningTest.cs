using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyMvcBackend.Migrations
{
    /// <inheritdoc />
    public partial class ListeningTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "ListeningTests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfQuestions",
                table: "ListeningTests",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "ListeningTests");

            migrationBuilder.DropColumn(
                name: "NumberOfQuestions",
                table: "ListeningTests");
        }
    }
}
