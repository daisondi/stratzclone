using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StratzClone.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    MatchId = table.Column<long>(type: "bigint", nullable: false),
                    DurationSecs = table.Column<int>(type: "int", nullable: false),
                    DidRadiantWin = table.Column<bool>(type: "bit", nullable: false),
                    StartDateUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.MatchId);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    SteamId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.SteamId);
                });

            migrationBuilder.CreateTable(
                name: "PlayerMatches",
                columns: table => new
                {
                    MatchId = table.Column<long>(type: "bigint", nullable: false),
                    SteamId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HeroId = table.Column<int>(type: "int", nullable: false),
                    Kills = table.Column<int>(type: "int", nullable: false),
                    Deaths = table.Column<int>(type: "int", nullable: false),
                    Assists = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerMatches", x => new { x.MatchId, x.SteamId });
                    table.ForeignKey(
                        name: "FK_PlayerMatches_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "MatchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerMatches_Players_SteamId",
                        column: x => x.SteamId,
                        principalTable: "Players",
                        principalColumn: "SteamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerMatchItems",
                columns: table => new
                {
                    MatchId = table.Column<long>(type: "bigint", nullable: false),
                    SteamId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ItemSeq = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    PurchaseTime = table.Column<int>(type: "int", nullable: false),
                    IsNeutral = table.Column<bool>(type: "bit", nullable: false),
                    Charges = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerMatchItems", x => new { x.MatchId, x.SteamId, x.ItemSeq });
                    table.ForeignKey(
                        name: "FK_PlayerMatchItems_PlayerMatches_MatchId_SteamId",
                        columns: x => new { x.MatchId, x.SteamId },
                        principalTable: "PlayerMatches",
                        principalColumns: new[] { "MatchId", "SteamId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerMatches_SteamId",
                table: "PlayerMatches",
                column: "SteamId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerMatchItems_ItemId",
                table: "PlayerMatchItems",
                column: "ItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerMatchItems");

            migrationBuilder.DropTable(
                name: "PlayerMatches");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
