using Microsoft.EntityFrameworkCore.Migrations;

namespace BojoBox.EntityFramework.Migrations
{
    public partial class mig2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Franchises_FranchiseId",
                table: "Teams");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Franchises_FranchiseId",
                table: "Teams",
                column: "FranchiseId",
                principalTable: "Franchises",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Franchises_FranchiseId",
                table: "Teams");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Franchises_FranchiseId",
                table: "Teams",
                column: "FranchiseId",
                principalTable: "Franchises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
