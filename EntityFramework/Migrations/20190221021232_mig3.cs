using Microsoft.EntityFrameworkCore.Migrations;

namespace BojoBox.EntityFramework.Migrations
{
    public partial class mig3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Franchises_CurrentTeamId",
                table: "Franchises");

            migrationBuilder.AlterColumn<int>(
                name: "CurrentTeamId",
                table: "Franchises",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Franchises_CurrentTeamId",
                table: "Franchises",
                column: "CurrentTeamId",
                unique: true,
                filter: "[CurrentTeamId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Franchises_CurrentTeamId",
                table: "Franchises");

            migrationBuilder.AlterColumn<int>(
                name: "CurrentTeamId",
                table: "Franchises",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Franchises_CurrentTeamId",
                table: "Franchises",
                column: "CurrentTeamId",
                unique: true);
        }
    }
}
