using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_TuyenDung.Migrations
{
    /// <inheritdoc />
    public partial class baithi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VerifyKey",
                table: "tbl_ViecLam",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerifyKey",
                table: "tbl_ViecLam");
        }
    }
}
