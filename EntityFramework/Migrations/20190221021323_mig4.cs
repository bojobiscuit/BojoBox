using Microsoft.EntityFrameworkCore.Migrations;

namespace BojoBox.EntityFramework.Migrations
{
    public partial class mig4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Franchises_Teams_CurrentTeamId",
                table: "Franchises");

            migrationBuilder.AddForeignKey(
                name: "FK_Franchises_Teams_CurrentTeamId",
                table: "Franchises",
                column: "CurrentTeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Franchises_Teams_CurrentTeamId",
                table: "Franchises");

            migrationBuilder.AddForeignKey(
                name: "FK_Franchises_Teams_CurrentTeamId",
                table: "Franchises",
                column: "CurrentTeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
