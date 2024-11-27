using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_TuyenDung.Migrations
{
    /// <inheritdoc />
    public partial class _8888 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_QuyenHan",
                columns: table => new
                {
                    iMaQuyen = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sTenQuyen = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_QuyenHan", x => x.iMaQuyen);
                });

            migrationBuilder.CreateTable(
                name: "tbl_ViecLam",
                columns: table => new
                {
                    iMaViecLam = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sTieuDe = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    sMota = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fMucLuong = table.Column<double>(type: "float", nullable: false),
                    dNgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dNgayHetHan = table.Column<DateTime>(type: "datetime2", nullable: false),
                    bTrangThai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_ViecLam", x => x.iMaViecLam);
                });

            migrationBuilder.CreateTable(
                name: "tbl_TaiKhoan",
                columns: table => new
                {
                    iMaTaiKhoan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sTaiKhoan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    sMatKhau = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    iMaQuyen = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_TaiKhoan", x => x.iMaTaiKhoan);
                    table.ForeignKey(
                        name: "FK_tbl_TaiKhoan_tbl_QuyenHan_iMaQuyen",
                        column: x => x.iMaQuyen,
                        principalTable: "tbl_QuyenHan",
                        principalColumn: "iMaQuyen",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_NguoiDung",
                columns: table => new
                {
                    iMaND = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sTenND = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    sEmail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    sSDT = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    dNgaySinh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    sGioiTinh = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    iMaTaiKhoan = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_NguoiDung", x => x.iMaND);
                    table.ForeignKey(
                        name: "FK_tbl_NguoiDung_tbl_TaiKhoan_iMaTaiKhoan",
                        column: x => x.iMaTaiKhoan,
                        principalTable: "tbl_TaiKhoan",
                        principalColumn: "iMaTaiKhoan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_DonUngTuyen",
                columns: table => new
                {
                    iMaDon = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    iMaViecLam = table.Column<int>(type: "int", nullable: false),
                    sMoTa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    iMaND = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_DonUngTuyen", x => x.iMaDon);
                    table.ForeignKey(
                        name: "FK_tbl_DonUngTuyen_tbl_NguoiDung_iMaND",
                        column: x => x.iMaND,
                        principalTable: "tbl_NguoiDung",
                        principalColumn: "iMaND",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_DonUngTuyen_tbl_ViecLam_iMaViecLam",
                        column: x => x.iMaViecLam,
                        principalTable: "tbl_ViecLam",
                        principalColumn: "iMaViecLam",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_ThongBao",
                columns: table => new
                {
                    iMaTB = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sToEmail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    sFromEmail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    sTieuDe = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    sNoiDung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    iMaND = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_ThongBao", x => x.iMaTB);
                    table.ForeignKey(
                        name: "FK_tbl_ThongBao_tbl_NguoiDung_iMaND",
                        column: x => x.iMaND,
                        principalTable: "tbl_NguoiDung",
                        principalColumn: "iMaND",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_DonUngTuyen_iMaND",
                table: "tbl_DonUngTuyen",
                column: "iMaND");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_DonUngTuyen_iMaViecLam",
                table: "tbl_DonUngTuyen",
                column: "iMaViecLam");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_NguoiDung_iMaTaiKhoan",
                table: "tbl_NguoiDung",
                column: "iMaTaiKhoan",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_TaiKhoan_iMaQuyen",
                table: "tbl_TaiKhoan",
                column: "iMaQuyen");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ThongBao_iMaND",
                table: "tbl_ThongBao",
                column: "iMaND");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_DonUngTuyen");

            migrationBuilder.DropTable(
                name: "tbl_ThongBao");

            migrationBuilder.DropTable(
                name: "tbl_ViecLam");

            migrationBuilder.DropTable(
                name: "tbl_NguoiDung");

            migrationBuilder.DropTable(
                name: "tbl_TaiKhoan");

            migrationBuilder.DropTable(
                name: "tbl_QuyenHan");
        }
    }
}
