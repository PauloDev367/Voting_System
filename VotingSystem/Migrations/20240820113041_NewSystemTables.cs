using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VotingSystem.Migrations
{
    /// <inheritdoc />
    public partial class NewSystemTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AgentId",
                table: "Votes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Voted",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Agen",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agen", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Votes_AgentId",
                table: "Votes",
                column: "AgentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_Agen_AgentId",
                table: "Votes",
                column: "AgentId",
                principalTable: "Agen",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Votes_Agen_AgentId",
                table: "Votes");

            migrationBuilder.DropTable(
                name: "Agen");

            migrationBuilder.DropIndex(
                name: "IX_Votes_AgentId",
                table: "Votes");

            migrationBuilder.DropColumn(
                name: "AgentId",
                table: "Votes");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Voted",
                table: "AspNetUsers");
        }
    }
}
