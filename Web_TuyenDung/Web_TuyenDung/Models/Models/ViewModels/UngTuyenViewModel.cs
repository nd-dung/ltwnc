using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Web_TuyenDung.Models.ViewModels
{
    public class UngTuyenViewModel
    {
        [Required(ErrorMessage = "Không được bỏ trống")]
        public string HoTen {  get; set; }

        [Required]
        public string Email {get; set; }

        [Required,RegularExpression(@"^\d+$", ErrorMessage = "Số điện thoại chỉ được chứa các chữ số.")]
        public string SDT {get; set; }

        [Required]
        public DateTime? NgaySinh { get; set; }

        [Required]
        public int MaViecLam { get; set; }

        [Required(ErrorMessage ="Không được bỏ trống")]
        public string MoTa { get; set; }

        [Required]
        public int MaND { get; set; }


    }
}
