using Web_TuyenDung.Models;

namespace Web_TuyenDung.Models.ViewModels
{
    public class JobViewModel
    {
        public IEnumerable<ViecLam> DanhSachViecLam { get; set; }
        public ViecLam ViecLamMoi { get; set; }
        public bool IsAdmin { get; set; }
    }
}