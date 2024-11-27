using System.ComponentModel.DataAnnotations;

namespace Web_TuyenDung.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="Không được bỏ trống")]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "Không được bỏ trống")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Không được bỏ trống")]
        public string MatKhau { get; set; }
    }
}
