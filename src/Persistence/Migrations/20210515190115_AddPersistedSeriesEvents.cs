using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Eumel.Persistance.Migrations
{
    public partial class AddPersistedSeriesEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SeriesEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GameUuid = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "varchar(30)", nullable: false),
                    Payload = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeriesEvents", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_RoundIndex",
                table: "Events",
                column: "RoundIndex");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesEvents_GameUuid",
                table: "SeriesEvents",
                column: "GameUuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SeriesEvents");

            migrationBuilder.DropIndex(
                name: "IX_Events_RoundIndex",
                table: "Events");
        }
    }
}
