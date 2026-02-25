using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TreineMais.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaInstrutorNoTreino : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InstrutorId",
                table: "Treinos",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Treinos_InstrutorId",
                table: "Treinos",
                column: "InstrutorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Treinos_AspNetUsers_InstrutorId",
                table: "Treinos",
                column: "InstrutorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Treinos_AspNetUsers_InstrutorId",
                table: "Treinos");

            migrationBuilder.DropIndex(
                name: "IX_Treinos_InstrutorId",
                table: "Treinos");

            migrationBuilder.DropColumn(
                name: "InstrutorId",
                table: "Treinos");
        }
    }
}
