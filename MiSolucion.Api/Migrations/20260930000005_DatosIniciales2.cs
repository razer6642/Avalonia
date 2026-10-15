using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MiSolucion.Api.Migrations
{
    /// <inheritdoc />
    public partial class DatosIniciales2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Clases",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Bárbaro" },
                    { 2, "Paladín" },
                    { 3, "Pícaro" }
                });

            migrationBuilder.InsertData(
                table: "Razas",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Humano" },
                    { 2, "Elfo" },
                    { 3, "Enano" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Clases",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Clases",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Clases",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Razas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Razas",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Razas",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
