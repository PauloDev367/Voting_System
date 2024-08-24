using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VotingSystem.Migrations
{
    /// <inheritdoc />
    public partial class HubIdTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Votes_Agen_AgentId",
                table: "Votes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Agen",
                table: "Agen");

            migrationBuilder.RenameTable(
                name: "Agen",
                newName: "Agents");

            migrationBuilder.AddColumn<bool>(
                name: "IsOnline",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Agents",
                table: "Agents",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "HubIds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientHubId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId1 = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HubIds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HubIds_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_HubIds_UserId1",
                table: "HubIds",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_Agents_AgentId",
                table: "Votes",
                column: "AgentId",
                principalTable: "Agents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Votes_Agents_AgentId",
                table: "Votes");

            migrationBuilder.DropTable(
                name: "HubIds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Agents",
                table: "Agents");

            migrationBuilder.DropColumn(
                name: "IsOnline",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "Agents",
                newName: "Agen");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Agen",
                table: "Agen",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_Agen_AgentId",
                table: "Votes",
                column: "AgentId",
                principalTable: "Agen",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
