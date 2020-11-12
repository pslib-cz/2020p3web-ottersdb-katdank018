using Microsoft.EntityFrameworkCore.Migrations;

namespace Database01.Migrations
{
    public partial class D : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FounderId",
                table: "Otters",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Otters_FounderId",
                table: "Otters",
                column: "FounderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Otters_AspNetUsers_FounderId",
                table: "Otters",
                column: "FounderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Otters_AspNetUsers_FounderId",
                table: "Otters");

            migrationBuilder.DropIndex(
                name: "IX_Otters_FounderId",
                table: "Otters");

            migrationBuilder.DropColumn(
                name: "FounderId",
                table: "Otters");
        }
    }
}
