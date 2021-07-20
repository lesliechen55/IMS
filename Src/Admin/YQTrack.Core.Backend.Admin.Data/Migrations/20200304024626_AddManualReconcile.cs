using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YQTrack.Core.Backend.Admin.Data.Migrations
{
    public partial class AddManualReconcile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TManualReconcile",
                columns: table => new
                {
                    FId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FFileName = table.Column<string>(maxLength: 50, nullable: false),
                    FFileMd5 = table.Column<string>(maxLength: 32, nullable: false),
                    FFilePath = table.Column<string>(maxLength: 250, nullable: false),
                    FYear = table.Column<int>(nullable: false),
                    FMonth = table.Column<int>(nullable: false),
                    FOrderCount = table.Column<int>(nullable: false),
                    FRemark = table.Column<string>(maxLength: 50, nullable: false),
                    FCreatedBy = table.Column<int>(nullable: false),
                    FCreatedTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TManualReconcile", x => x.FId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TManualReconcile");
        }
    }
}
