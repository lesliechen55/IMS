using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YQTrack.Core.Backend.Admin.Data.Migrations
{
    public partial class AddOperationLogTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TLoginLog_FAccount_FNickName_FPlatform_FUserAgent_FIp",
                table: "TLoginLog");

            migrationBuilder.CreateTable(
                name: "TOperationLog",
                columns: table => new
                {
                    FId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FManagerId = table.Column<int>(nullable: false),
                    FAccount = table.Column<string>(maxLength: 16, nullable: false),
                    FNickName = table.Column<string>(maxLength: 16, nullable: false),
                    FIp = table.Column<string>(maxLength: 16, nullable: false),
                    FMethod = table.Column<string>(maxLength: 32, nullable: false),
                    FParameter = table.Column<string>(maxLength: 1000, nullable: false),
                    FDesc = table.Column<string>(maxLength: 16, nullable: false),
                    FOperationType = table.Column<int>(nullable: false),
                    FCreatedTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()"),
                    FCreatedBy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TOperationLog", x => x.FId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TLoginLog_FAccount",
                table: "TLoginLog",
                column: "FAccount");

            migrationBuilder.CreateIndex(
                name: "IX_TLoginLog_FLoginTime",
                table: "TLoginLog",
                column: "FLoginTime");

            migrationBuilder.CreateIndex(
                name: "IX_TLoginLog_FNickName",
                table: "TLoginLog",
                column: "FNickName");

            migrationBuilder.CreateIndex(
                name: "IX_TLoginLog_FPlatform",
                table: "TLoginLog",
                column: "FPlatform");

            migrationBuilder.CreateIndex(
                name: "IX_TOperationLog_FAccount",
                table: "TOperationLog",
                column: "FAccount");

            migrationBuilder.CreateIndex(
                name: "IX_TOperationLog_FCreatedTime",
                table: "TOperationLog",
                column: "FCreatedTime");

            migrationBuilder.CreateIndex(
                name: "IX_TOperationLog_FMethod",
                table: "TOperationLog",
                column: "FMethod");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TOperationLog");

            migrationBuilder.DropIndex(
                name: "IX_TLoginLog_FAccount",
                table: "TLoginLog");

            migrationBuilder.DropIndex(
                name: "IX_TLoginLog_FLoginTime",
                table: "TLoginLog");

            migrationBuilder.DropIndex(
                name: "IX_TLoginLog_FNickName",
                table: "TLoginLog");

            migrationBuilder.DropIndex(
                name: "IX_TLoginLog_FPlatform",
                table: "TLoginLog");

            migrationBuilder.CreateIndex(
                name: "IX_TLoginLog_FAccount_FNickName_FPlatform_FUserAgent_FIp",
                table: "TLoginLog",
                columns: new[] { "FAccount", "FNickName", "FPlatform", "FUserAgent", "FIp" });
        }
    }
}
