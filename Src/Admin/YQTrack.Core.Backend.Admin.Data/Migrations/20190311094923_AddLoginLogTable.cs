using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YQTrack.Core.Backend.Admin.Data.Migrations
{
    public partial class AddLoginLogTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TLoginLog",
                columns: table => new
                {
                    FId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FManagerId = table.Column<int>(nullable: false),
                    FAccount = table.Column<string>(maxLength: 16, nullable: false),
                    FNickName = table.Column<string>(maxLength: 16, nullable: false),
                    FIp = table.Column<string>(maxLength: 16, nullable: false),
                    FPlatform = table.Column<string>(maxLength: 16, nullable: false),
                    FUserAgent = table.Column<string>(maxLength: 64, nullable: false),
                    FLoginTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()"),
                    FCreatedTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()"),
                    FCreatedBy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TLoginLog", x => x.FId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TLoginLog_FAccount_FNickName_FPlatform_FUserAgent_FIp",
                table: "TLoginLog",
                columns: new[] { "FAccount", "FNickName", "FPlatform", "FUserAgent", "FIp" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TLoginLog");
        }
    }
}
