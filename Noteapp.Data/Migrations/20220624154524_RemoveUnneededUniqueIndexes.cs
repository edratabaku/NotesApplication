using Microsoft.EntityFrameworkCore.Migrations;

namespace Noteapp.Data.Migrations
{
    public partial class RemoveUnneededUniqueIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropIndex("IX_AspNetUsers_CreatedById", "AspNetUsers", "dbo");

            migrationBuilder.DropIndex("IX_AspNetUsers_UpdatedById", "AspNetUsers", "dbo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CreatedById",
                table: "AspNetUsers",
                column: "CreatedById",
                unique: true,
                filter: "[CreatedById] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UpdatedById",
                table: "AspNetUsers",
                column: "UpdatedById",
                unique: true,
                filter: "[UpdatedById] IS NOT NULL");
        }
    }
}
