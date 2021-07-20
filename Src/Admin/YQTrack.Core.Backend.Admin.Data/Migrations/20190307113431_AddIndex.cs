using Microsoft.EntityFrameworkCore.Migrations;

namespace YQTrack.Core.Backend.Admin.Data.Migrations
{
    public partial class AddIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TRole_FName",
                table: "TRole",
                column: "FName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TPermission_FFullName",
                table: "TPermission",
                column: "FFullName");

            migrationBuilder.CreateIndex(
                name: "IX_TPermission_FName",
                table: "TPermission",
                column: "FName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TPermission_FUrl",
                table: "TPermission",
                column: "FUrl");

            migrationBuilder.CreateIndex(
                name: "IX_TManager_FAccount",
                table: "TManager",
                column: "FAccount",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TRole_FName",
                table: "TRole");

            migrationBuilder.DropIndex(
                name: "IX_TPermission_FFullName",
                table: "TPermission");

            migrationBuilder.DropIndex(
                name: "IX_TPermission_FName",
                table: "TPermission");

            migrationBuilder.DropIndex(
                name: "IX_TPermission_FUrl",
                table: "TPermission");

            migrationBuilder.DropIndex(
                name: "IX_TManager_FAccount",
                table: "TManager");
        }
    }
}
