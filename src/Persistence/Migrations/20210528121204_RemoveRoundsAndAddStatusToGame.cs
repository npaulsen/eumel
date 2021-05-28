using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Eumel.Persistance.Migrations
{
    public partial class RemoveRoundsAndAddStatusToGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rounds");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Games",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Games");

            migrationBuilder.CreateTable(
                name: "Rounds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Index = table.Column<int>(type: "integer", nullable: false),
                    NumTricks = table.Column<int>(type: "integer", nullable: false),
                    PersistedEumelGameId = table.Column<int>(type: "integer", nullable: true),
                    StartingPlayerIndex = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rounds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rounds_Games_PersistedEumelGameId",
                        column: x => x.PersistedEumelGameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_PersistedEumelGameId",
                table: "Rounds",
                column: "PersistedEumelGameId");
        }
    }
}
