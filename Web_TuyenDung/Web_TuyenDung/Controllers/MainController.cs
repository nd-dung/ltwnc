using Microsoft.AspNetCore.Mvc;
using Web_TuyenDung.DAO;
using Web_TuyenDung.Models;
using Web_TuyenDung.Models.ViewModels;

namespace Web_TuyenDung.Controllers
{
    public class MainController : Controller
    {
        private readonly ViecLamDAO _ViecLamDAO;
        private readonly NguoiDungDAO _NguoiDungDAO;
        private readonly UngTuyenDAO _UngTuyenDAO;

        public MainController(ViecLamDAO viecLamDAO, NguoiDungDAO nguoiDungDAO, UngTuyenDAO ungTuyenDAO)
        {
            _ViecLamDAO = viecLamDAO;
            _NguoiDungDAO = nguoiDungDAO;
            _UngTuyenDAO = ungTuyenDAO;
        }
        public async Task<IActionResult> ViewMain()
        {
            var viewModel = new JobViewModel
            {
                DanhSachViecLam = await _ViecLamDAO.GetAll(),
                ViecLamMoi = new ViecLam(),

                IsAdmin = HttpContext.Session.GetString("QuyenHan")?.Equals("Admin") ?? false
            };
            return View(viewModel);
        }

        [HttpPost]
        [Route("ThemViecLam")]
        public async Task<JsonResult> ThemViecLam(ViecLam model)
        {
            try
            {
                // Kiểm tra session
                var ndjson = HttpContext.Session.GetString("NguoiDung");
                if (ndjson == null)
                {
                    return Json(new { success = false, message = "Vui lòng đăng nhập!" });
                }

                // Kiểm tra quyền
                var quyen = HttpContext.Session.GetString("QuyenHan");
                if (quyen == null || !quyen.Equals("Admin"))
                {
                    return Json(new { success = false, message = "Không có quyền thực hiện!" });
                }

                // if (!ModelState.IsValid)
                // {
                //     return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });
                // }

                // if (model.NgayHetHan <= model.NgayTao)
                // {
                //     return Json(new { success = false, message = "Ngày hết hạn phải lớn hơn ngày tạo!" });
                // }

                var viecLam = new ViecLam
                {
                    TieuDe = model.TieuDe,
                    MoTa = model.MoTa,
                    TTLienHe = model.TTLienHe,
                    MucLuong = model.MucLuong,
                    NgayTao = model.NgayTao,
                    NgayHetHan = model.NgayHetHan,
                    TrangThai = model.TrangThai,
                };

                await _ViecLamDAO.Save(viecLam);
                return Json(new { success = true, message = "Thêm việc làm thành công!" });
            }
            catch (Exception ex)
            {
                // Log the full exception details
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                return Json(new { success = false, message = ex.InnerException?.Message ?? ex.Message });
            }
        }

        //xóa việc làm 
        [HttpPost]
        [Route("XoaViecLam/{id_vieclam}")]
        public async Task<IActionResult> XoaViecLam(int id_vieclam)
        {
            var quyen = HttpContext.Session.GetString("QuyenHan");
            if (quyen == null || !quyen.Equals("Admin"))
            {
                return Json(new { success = false, message = "Không có quyền thực hiện!" });
            }
            bool result = await _ViecLamDAO.Delete(id_vieclam);
            if (result)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Xóa việc làm không thành công." });
        }
    }
}
