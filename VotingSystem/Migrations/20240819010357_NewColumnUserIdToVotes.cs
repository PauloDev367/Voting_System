using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VotingSystem.Migrations
{
    /// <inheritdoc />
    public partial class NewColumnUserIdToVotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Votes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Votes");
        }
    }
}
