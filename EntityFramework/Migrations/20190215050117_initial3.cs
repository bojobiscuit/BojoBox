using Microsoft.EntityFrameworkCore.Migrations;

namespace BojoBox.EntityFramework.Migrations
{
    public partial class initial3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmptyNetGoals",
                table: "SkaterSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FaceoffWins",
                table: "SkaterSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FaceoffsTotal",
                table: "SkaterSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FightsDraw",
                table: "SkaterSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FightsLost",
                table: "SkaterSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FightsWon",
                table: "SkaterSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GameTyingGoals",
                table: "SkaterSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GameWinningGoals",
                table: "SkaterSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HatTricks",
                table: "SkaterSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Hits",
                table: "SkaterSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HitsTaken",
                table: "SkaterSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinutesPlayed",
                table: "SkaterSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OwnShotsBlocked",
                table: "SkaterSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OwnShotsMissed",
                table: "SkaterSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PKAssists",
                table: "SkaterSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PKGoals",
                table: "SkaterSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PKMinutes",
                table: "SkaterSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PKPoints",
                table: "SkaterSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PKShots",
                table: "SkaterSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PPAssists",
                table: "SkaterSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PPGoals",
                table: "SkaterSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PPMinutes",
                table: "SkaterSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PPPoints",
                table: "SkaterSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PPShots",
                table: "SkaterSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PenaltyMajors",
                table: "SkaterSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PenaltyMinutes",
                table: "SkaterSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PenaltyShotAttempts",
                table: "SkaterSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PenaltyShotGoals",
                table: "SkaterSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlusMinus",
                table: "SkaterSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "SkaterSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Shots",
                table: "SkaterSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShotsBlocked",
                table: "SkaterSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Assists",
                table: "GoalieSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Backups",
                table: "GoalieSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EmptyGoalAgainst",
                table: "GoalieSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GoalsAgainst",
                table: "GoalieSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Minutes",
                table: "GoalieSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OvertimeLosses",
                table: "GoalieSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PenaltyMinutes",
                table: "GoalieSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PenaltyShotAttempts",
                table: "GoalieSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PenaltyShotSaves",
                table: "GoalieSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShotsAgainst",
                table: "GoalieSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Shutouts",
                table: "GoalieSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Starts",
                table: "GoalieSeasons",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmptyNetGoals",
                table: "SkaterSeasons");

            migrationBuilder.DropColumn(
                name: "FaceoffWins",
                table: "SkaterSeasons");

            migrationBuilder.DropColumn(
                name: "FaceoffsTotal",
                table: "SkaterSeasons");

            migrationBuilder.DropColumn(
                name: "FightsDraw",
                table: "SkaterSeasons");

            migrationBuilder.DropColumn(
                name: "FightsLost",
                table: "SkaterSeasons");

            migrationBuilder.DropColumn(
                name: "FightsWon",
                table: "SkaterSeasons");

            migrationBuilder.DropColumn(
                name: "GameTyingGoals",
                table: "SkaterSeasons");

            migrationBuilder.DropColumn(
                name: "GameWinningGoals",
                table: "SkaterSeasons");

            migrationBuilder.DropColumn(
                name: "HatTricks",
                table: "SkaterSeasons");

            migrationBuilder.DropColumn(
                name: "Hits",
                table: "SkaterSeasons");

            migrationBuilder.DropColumn(
                name: "HitsTaken",
                table: "SkaterSeasons");

            migrationBuilder.DropColumn(
                name: "MinutesPlayed",
                table: "SkaterSeasons");

            migrationBuilder.DropColumn(
                name: "OwnShotsBlocked",
                table: "SkaterSeasons");

            migrationBuilder.DropColumn(
                name: "OwnShotsMissed",
                table: "SkaterSeasons");

            migrationBuilder.DropColumn(
                name: "PKAssists",
                table: "SkaterSeasons");

            migrationBuilder.DropColumn(
                name: "PKGoals",
                table: "SkaterSeasons");

            migrationBuilder.DropColumn(
                name: "PKMinutes",
                table: "SkaterSeasons");

            migrationBuilder.DropColumn(
                name: "PKPoints",
                table: "SkaterSeasons");

            migrationBuilder.DropColumn(
                name: "PKShots",
                table: "SkaterSeasons");

            migrationBuilder.DropColumn(
                name: "PPAssists",
                table: "SkaterSeasons");

            migrationBuilder.DropColumn(
                name: "PPGoals",
                table: "SkaterSeasons");

            migrationBuilder.DropColumn(
                name: "PPMinutes",
                table: "SkaterSeasons");

            migrationBuilder.DropColumn(
                name: "PPPoints",
                table: "SkaterSeasons");

            migrationBuilder.DropColumn(
                name: "PPShots",
                table: "SkaterSeasons");

            migrationBuilder.DropColumn(
                name: "PenaltyMajors",
                table: "SkaterSeasons");

            migrationBuilder.DropColumn(
                name: "PenaltyMinutes",
                table: "SkaterSeasons");

            migrationBuilder.DropColumn(
                name: "PenaltyShotAttempts",
                table: "SkaterSeasons");

            migrationBuilder.DropColumn(
                name: "PenaltyShotGoals",
                table: "SkaterSeasons");

            migrationBuilder.DropColumn(
                name: "PlusMinus",
                table: "SkaterSeasons");

            migrationBuilder.DropColumn(
                name: "Points",
                table: "SkaterSeasons");

            migrationBuilder.DropColumn(
                name: "Shots",
                table: "SkaterSeasons");

            migrationBuilder.DropColumn(
                name: "ShotsBlocked",
                table: "SkaterSeasons");

            migrationBuilder.DropColumn(
                name: "Assists",
                table: "GoalieSeasons");

            migrationBuilder.DropColumn(
                name: "Backups",
                table: "GoalieSeasons");

            migrationBuilder.DropColumn(
                name: "EmptyGoalAgainst",
                table: "GoalieSeasons");

            migrationBuilder.DropColumn(
                name: "GoalsAgainst",
                table: "GoalieSeasons");

            migrationBuilder.DropColumn(
                name: "Minutes",
                table: "GoalieSeasons");

            migrationBuilder.DropColumn(
                name: "OvertimeLosses",
                table: "GoalieSeasons");

            migrationBuilder.DropColumn(
                name: "PenaltyMinutes",
                table: "GoalieSeasons");

            migrationBuilder.DropColumn(
                name: "PenaltyShotAttempts",
                table: "GoalieSeasons");

            migrationBuilder.DropColumn(
                name: "PenaltyShotSaves",
                table: "GoalieSeasons");

            migrationBuilder.DropColumn(
                name: "ShotsAgainst",
                table: "GoalieSeasons");

            migrationBuilder.DropColumn(
                name: "Shutouts",
                table: "GoalieSeasons");

            migrationBuilder.DropColumn(
                name: "Starts",
                table: "GoalieSeasons");
        }
    }
}
