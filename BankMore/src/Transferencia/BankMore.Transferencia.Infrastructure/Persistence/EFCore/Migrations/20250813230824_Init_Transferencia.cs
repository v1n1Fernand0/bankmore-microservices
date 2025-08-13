using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankMore.Transferencia.Infrastructure.Persistence.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class Init_Transferencia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "idempotencia",
                columns: table => new
                {
                    chave_idempotencia = table.Column<string>(type: "TEXT", maxLength: 37, nullable: false),
                    requisicao = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    resultado = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_idempotencia", x => x.chave_idempotencia);
                });

            migrationBuilder.CreateTable(
                name: "transferencia",
                columns: table => new
                {
                    idtransferencia = table.Column<string>(type: "TEXT", maxLength: 37, nullable: false),
                    idcontacorrente_origem = table.Column<string>(type: "TEXT", maxLength: 37, nullable: false),
                    idcontacorrente_destino = table.Column<long>(type: "INTEGER", nullable: false),
                    valor = table.Column<decimal>(type: "TEXT", nullable: false),
                    datamovimento = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transferencia", x => x.idtransferencia);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "idempotencia");

            migrationBuilder.DropTable(
                name: "transferencia");
        }
    }
}
