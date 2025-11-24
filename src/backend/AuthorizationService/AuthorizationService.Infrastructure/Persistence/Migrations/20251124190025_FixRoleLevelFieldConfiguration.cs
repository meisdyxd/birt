using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthorizationService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixRoleLevelFieldConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Level",
                table: "roles",
                newName: "level");

            migrationBuilder.AlterColumn<int>(
                name: "level",
                table: "roles",
                type: "integer",
                nullable: false,
                comment: "Уровень роли",
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "level",
                table: "roles",
                newName: "Level");

            migrationBuilder.AlterColumn<int>(
                name: "Level",
                table: "roles",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "Уровень роли");
        }
    }
}
