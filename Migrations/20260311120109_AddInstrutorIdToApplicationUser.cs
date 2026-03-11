using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TreineMais.Migrations
{
    /// <inheritdoc />
    public partial class AddInstrutorIdToApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InstrutorId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_InstrutorId",
                table: "AspNetUsers",
                column: "InstrutorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_InstrutorId",
                table: "AspNetUsers",
                column: "InstrutorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_InstrutorId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_InstrutorId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "InstrutorId",
                table: "AspNetUsers");
        }
    }
}
