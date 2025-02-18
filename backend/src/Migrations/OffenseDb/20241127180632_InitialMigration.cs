using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace src.Migrations.OffenseDb
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OffenseRecords",
                columns: table => new
                {
                    RecordId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HausnummerInt = table.Column<int>(type: "INTEGER", nullable: false),
                    HausnummerExtra = table.Column<string>(type: "TEXT", nullable: false),
                    RowVersion = table.Column<int>(type: "INTEGER", nullable: false),
                    Weiserzeichen = table.Column<string>(type: "TEXT", nullable: false),
                    Fallnummer = table.Column<string>(type: "TEXT", nullable: false),
                    Meldedatum = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Versicherungsunternehmensnummer = table.Column<string>(type: "TEXT", nullable: false),
                    Krankenversicherung = table.Column<string>(type: "TEXT", nullable: false),
                    Versicherungsnummer = table.Column<string>(type: "TEXT", nullable: false),
                    BeginnVersicherung = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Geschlecht = table.Column<string>(type: "TEXT", nullable: false),
                    Titel = table.Column<string>(type: "TEXT", nullable: false),
                    Geburtsdatum = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Vorname = table.Column<string>(type: "TEXT", nullable: false),
                    Nachname = table.Column<string>(type: "TEXT", nullable: false),
                    Geburtsname = table.Column<string>(type: "TEXT", nullable: false),
                    Str = table.Column<string>(type: "TEXT", nullable: false),
                    Hausnummer = table.Column<string>(type: "TEXT", nullable: false),
                    Plz = table.Column<string>(type: "TEXT", nullable: false),
                    Wohnort = table.Column<string>(type: "TEXT", nullable: false),
                    Ortsteil = table.Column<string>(type: "TEXT", nullable: false),
                    Geburtsort = table.Column<string>(type: "TEXT", nullable: false),
                    Aufforderungsdatum = table.Column<DateTime>(type: "TEXT", nullable: false),
                    BeginnRueckstand = table.Column<DateTime>(type: "TEXT", nullable: false),
                    VerzugBis = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Verzugsende = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Beitragsrueckstand = table.Column<string>(type: "TEXT", nullable: false),
                    Sollbeitrag = table.Column<string>(type: "TEXT", nullable: false),
                    Folgemeldung = table.Column<int>(type: "INTEGER", nullable: false),
                    Bemerkungen = table.Column<string>(type: "TEXT", nullable: false),
                    Anhoerungsdatum = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OffenseRecords", x => x.RecordId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OffenseRecords");
        }
    }
}
