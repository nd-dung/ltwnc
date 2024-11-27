using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Web_TuyenDung.Models.ViewModels
{
    public class ViecLamViewModel
    {
        [Required(ErrorMessage = "Không được bỏ trống")]
        [RegularExpression(@"^\d.*", ErrorMessage = "Tiêu đề phải bắt đầu bằng chữ số")]
        [MinLength(10, ErrorMessage = "Tiêu đề phải có ít nhất 10 kí tự")]
        public string TieuDe { get; set; }

        [Required(ErrorMessage = "Không được bỏ trống")]
        public string MoTa { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống")]
        public string TTLienHe { get; set; }

        [Required(ErrorMessage = "Không được bỏ trống"), Range(1, float.MaxValue, ErrorMessage = "Mức lương không hợp lệ")]
        public float MucLuong { get; set; }

        [Required(ErrorMessage = "Không được bỏ trống")]
        public DateTime NgayTao { get; set; }

        [Required(ErrorMessage = "Không được bỏ trống")]
        public DateTime NgayHetHan { get; set; }

        public int TrangThai { get; set; }

    }
}
