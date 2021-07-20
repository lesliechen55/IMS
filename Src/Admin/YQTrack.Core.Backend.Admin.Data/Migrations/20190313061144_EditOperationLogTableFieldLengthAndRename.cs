using Microsoft.EntityFrameworkCore.Migrations;

namespace YQTrack.Core.Backend.Admin.Data.Migrations
{
    public partial class EditOperationLogTableFieldLengthAndRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FManagerId",
                table: "TOperationLog",
                newName: "FOperatorId");

            migrationBuilder.AlterColumn<string>(
                name: "FParameter",
                table: "TOperationLog",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "FMethod",
                table: "TOperationLog",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 32);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FOperatorId",
                table: "TOperationLog",
                newName: "FManagerId");

            migrationBuilder.AlterColumn<string>(
                name: "FParameter",
                table: "TOperationLog",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FMethod",
                table: "TOperationLog",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 64);
        }
    }
}
