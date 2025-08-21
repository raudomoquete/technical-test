using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DGII.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contribuyentes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    rncCedula = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    tipo = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    estatus = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Contribu__3214EC070D5622E1", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComprobantesFiscales",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    ContribuyenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NCF = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    monto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    itbis18 = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Comproba__3214EC0727DE92D3", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComprobantesFiscales_Contribuyentes",
                        column: x => x.ContribuyenteId,
                        principalTable: "Contribuyentes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComprobantesFiscales_ContribuyenteId",
                table: "ComprobantesFiscales",
                column: "ContribuyenteId");

            migrationBuilder.CreateIndex(
                name: "IX_ComprobantesFiscales_NCF",
                table: "ComprobantesFiscales",
                column: "NCF",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contribuyentes_estatus",
                table: "Contribuyentes",
                column: "estatus");

            migrationBuilder.CreateIndex(
                name: "IX_Contribuyentes_rncCedula",
                table: "Contribuyentes",
                column: "rncCedula",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComprobantesFiscales");

            migrationBuilder.DropTable(
                name: "Contribuyentes");
        }
    }
}
