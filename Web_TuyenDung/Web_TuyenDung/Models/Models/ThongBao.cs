using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_TuyenDung.Models
{
    [Table("tbl_ThongBao")]
    public class ThongBao
    {
        [Key,Column("iMaTB"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaTB { get; set; }

        [Column("sToEmail"), Required, StringLength(50)]
        public string ToEmail { get; set; }

        [Column("sFromEmail"), Required, StringLength(50)]
        public string FromEmail { get; set; }

        [Column("sTieuDe"), Required, StringLength(100)]
        public string TieuDe { get; set; }

        [Column("sNoiDung", TypeName ="nvarchar(max)"), Required]
        public string NoiDung { get; set; }

        public int iMaND {  get; set; }

        [ForeignKey("iMaND"), Required]
        public NguoiDung NguoiDung { get; set; }


    }
}
