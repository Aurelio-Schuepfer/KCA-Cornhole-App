using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KCA_TournamentAPI.Migrations
{
    /// <inheritdoc />
    public partial class TournamentSponsorPartnerNullableFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Partners_Tournaments_TournamentId",
                table: "Partners");

            migrationBuilder.DropForeignKey(
                name: "FK_Sponsors_Tournaments_TournamentId",
                table: "Sponsors");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Sponsors");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Partners");

            migrationBuilder.AlterColumn<int>(
                name: "TournamentId",
                table: "Sponsors",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "Sponsors",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "TournamentId",
                table: "Partners",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "Partners",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Partners_Tournaments_TournamentId",
                table: "Partners",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sponsors_Tournaments_TournamentId",
                table: "Sponsors",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Partners_Tournaments_TournamentId",
                table: "Partners");

            migrationBuilder.DropForeignKey(
                name: "FK_Sponsors_Tournaments_TournamentId",
                table: "Sponsors");

            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "Sponsors");

            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "Partners");

            migrationBuilder.AlterColumn<int>(
                name: "TournamentId",
                table: "Sponsors",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Sponsors",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "TournamentId",
                table: "Partners",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Partners",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Partners_Tournaments_TournamentId",
                table: "Partners",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sponsors_Tournaments_TournamentId",
                table: "Sponsors",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id");
        }
    }
}
