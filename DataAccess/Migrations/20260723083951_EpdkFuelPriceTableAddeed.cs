using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class EpdkFuelPriceTableAddeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EpdkFuelPrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Fiyat = table.Column<decimal>(type: "numeric(15,5)", nullable: false),
                    Yakit = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Tarih = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    OlcuBirimi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PriceDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EpdkFuelPrices", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EpdkFuelPrices_Yakit_PriceDate",
                table: "EpdkFuelPrices",
                columns: new[] { "Yakit", "PriceDate" },
                unique: true,
                filter: "\"IsDeleted\" = false");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EpdkFuelPrices");
        }
    }
}
