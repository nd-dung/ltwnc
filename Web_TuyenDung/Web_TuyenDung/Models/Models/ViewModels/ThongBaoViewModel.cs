using System.ComponentModel.DataAnnotations;

namespace Web_TuyenDung.Models.ViewModels
{
    public class ThongBaoViewModel
    {
        [Required(ErrorMessage = "Không được bỏ trống")]
        public string ToEmail { get; set; }

        [Required(ErrorMessage = "Không được bỏ trống")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Không được bỏ trống")]
        public string Message { get; set; }

    }
}
