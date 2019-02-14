﻿using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BojoBox.EntityFramework.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Leagues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Acronym = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leagues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Goalies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LeagueId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goalies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Goalies_Leagues_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "Leagues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Skaters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LeagueId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skaters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skaters_Leagues_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "Leagues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FranchiseId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Acronym = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Franchises",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LeagueId = table.Column<int>(nullable: false),
                    CurrentTeamId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Franchises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Franchises_Teams_CurrentTeamId",
                        column: x => x.CurrentTeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Franchises_Leagues_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "Leagues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoalieSeasons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GoalieId = table.Column<int>(nullable: false),
                    TeamId = table.Column<int>(nullable: true),
                    SubtotalForId = table.Column<int>(nullable: true),
                    Season = table.Column<int>(nullable: false),
                    isPlayoffs = table.Column<bool>(nullable: false),
                    GamesPlayed = table.Column<int>(nullable: false),
                    Wins = table.Column<int>(nullable: false),
                    Losses = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoalieSeasons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoalieSeasons_Goalies_GoalieId",
                        column: x => x.GoalieId,
                        principalTable: "Goalies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoalieSeasons_GoalieSeasons_SubtotalForId",
                        column: x => x.SubtotalForId,
                        principalTable: "GoalieSeasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GoalieSeasons_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SkaterSeasons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SkaterId = table.Column<int>(nullable: false),
                    TeamId = table.Column<int>(nullable: true),
                    SubtotalForId = table.Column<int>(nullable: true),
                    Season = table.Column<int>(nullable: false),
                    isPlayoffs = table.Column<bool>(nullable: false),
                    GamesPlayed = table.Column<int>(nullable: false),
                    Goals = table.Column<int>(nullable: false),
                    Assists = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkaterSeasons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkaterSeasons_Skaters_SkaterId",
                        column: x => x.SkaterId,
                        principalTable: "Skaters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SkaterSeasons_SkaterSeasons_SubtotalForId",
                        column: x => x.SubtotalForId,
                        principalTable: "SkaterSeasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SkaterSeasons_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Franchises_CurrentTeamId",
                table: "Franchises",
                column: "CurrentTeamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Franchises_LeagueId",
                table: "Franchises",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_Goalies_LeagueId",
                table: "Goalies",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_GoalieSeasons_GoalieId",
                table: "GoalieSeasons",
                column: "GoalieId");

            migrationBuilder.CreateIndex(
                name: "IX_GoalieSeasons_SubtotalForId",
                table: "GoalieSeasons",
                column: "SubtotalForId");

            migrationBuilder.CreateIndex(
                name: "IX_GoalieSeasons_TeamId",
                table: "GoalieSeasons",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Skaters_LeagueId",
                table: "Skaters",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_SkaterSeasons_SkaterId",
                table: "SkaterSeasons",
                column: "SkaterId");

            migrationBuilder.CreateIndex(
                name: "IX_SkaterSeasons_SubtotalForId",
                table: "SkaterSeasons",
                column: "SubtotalForId");

            migrationBuilder.CreateIndex(
                name: "IX_SkaterSeasons_TeamId",
                table: "SkaterSeasons",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_FranchiseId",
                table: "Teams",
                column: "FranchiseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Franchises_FranchiseId",
                table: "Teams",
                column: "FranchiseId",
                principalTable: "Franchises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Franchises_Teams_CurrentTeamId",
                table: "Franchises");

            migrationBuilder.DropTable(
                name: "GoalieSeasons");

            migrationBuilder.DropTable(
                name: "SkaterSeasons");

            migrationBuilder.DropTable(
                name: "Goalies");

            migrationBuilder.DropTable(
                name: "Skaters");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Franchises");

            migrationBuilder.DropTable(
                name: "Leagues");
        }
    }
}
