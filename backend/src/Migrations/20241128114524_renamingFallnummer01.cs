using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace src.Migrations.WzDb
{
    /// <inheritdoc />
    public partial class renamingFallnummer01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Aktenzeichen",
                table: "WzEntities",
                newName: "Fallnummer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Fallnummer",
                table: "WzEntities",
                newName: "Aktenzeichen");
        }
    }
}
