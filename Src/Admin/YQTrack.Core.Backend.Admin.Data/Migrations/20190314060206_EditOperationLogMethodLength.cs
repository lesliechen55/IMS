using Microsoft.EntityFrameworkCore.Migrations;

namespace YQTrack.Core.Backend.Admin.Data.Migrations
{
    public partial class EditOperationLogMethodLength : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FMethod",
                table: "TOperationLog",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 64);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FMethod",
                table: "TOperationLog",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);
        }
    }
}
