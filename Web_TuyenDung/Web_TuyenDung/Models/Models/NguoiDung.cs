using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Web_TuyenDung.Models
{
    [Table("tbl_NguoiDung")]
    public class NguoiDung
    {
        [Key, Column("iMaND"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaND { get; set; }

        [Column("sTenND"), Required, StringLength(50)]
        public string TenND { set; get; }

        [Column("sEmail"), Required, StringLength(50)]
        public string Email {set; get;}

        [Column("sSDT"), StringLength(15)]
        public string? SDT {  set; get; }

        [Column("dNgaySinh")]
        public DateTime? NgaySinh { set; get; }

        [Column("sGioiTinh"), StringLength(10)]
        public string? GioiTinh {  set; get; }

        public int iMaTaiKhoan { get; set; }

        [JsonIgnore]
        [ForeignKey("iMaTaiKhoan"), Required]
        public TaiKhoan TaiKhoan { get; set; }



    }
}
