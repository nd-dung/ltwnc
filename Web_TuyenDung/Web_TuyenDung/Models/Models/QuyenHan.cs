using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_TuyenDung.Models
{
    [Table("tbl_QuyenHan")]
    public class QuyenHan
    {
        [Key, Column("iMaQuyen"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaQuyen {  get; set; }

        [Column("sTenQuyen"), Required, StringLength(20)]        
        public string TenQuyen { get; set; }



    }
}
