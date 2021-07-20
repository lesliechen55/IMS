using Microsoft.EntityFrameworkCore.Migrations;

namespace YQTrack.Core.Backend.Admin.Data.Migrations
{
    public partial class UpdatePermissonAreaAndRoleIsActiveFiled : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "FIsActive",
                table: "TRole",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "FAreaName",
                table: "TPermission",
                maxLength: 16,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 16);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "FIsActive",
                table: "TRole",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "FAreaName",
                table: "TPermission",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 16,
                oldNullable: true);
        }
    }
}
