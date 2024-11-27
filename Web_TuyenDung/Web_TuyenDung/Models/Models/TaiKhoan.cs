using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Web_TuyenDung.Models
{
    [Table("tbl_TaiKhoan")]
    public class TaiKhoan
    {
        [Key, Column("iMaTaiKhoan"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaTaiKhoan { get; set; }

        [Column("sTaiKhoan"), Required, StringLength(50)]
        public string TenTaiKhoan { get; set; }

        [Column("sMatKhau"), Required, StringLength(50)]
        public string MatKhau { get; set; }

        public NguoiDung NguoiDung { get; set; }

        public int iMaQuyen { get; set; }

        [ForeignKey("iMaQuyen"), Required]
        public QuyenHan QuyenHan { get; set; }

    }
}
