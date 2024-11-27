using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_TuyenDung.Models
{
    [Table("tbl_DonUngTuyen")]
    public class DonUngTuyen
    {
        [Key,Column("iMaDon"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaDon {  get; set; }

        public int iMaViecLam { get; set; }

        [Column("sMoTa", TypeName = "nvarchar(max)"), Required]
        public string MoTa { get; set; }

        [ForeignKey("iMaViecLam"), Required]
        public ViecLam ViecLam { get; set; }

        public int iMaND { get; set; }

        [ForeignKey("iMaND"), Required]
        public NguoiDung NguoiDung { get; set; }


    }
}
