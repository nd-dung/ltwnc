using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Web_TuyenDung.DAO;
using Web_TuyenDung.Models;
using Web_TuyenDung.Models.ViewModels;

namespace Web_TuyenDung.Controllers
{
    [Route("Admin/")]
    public class AdminController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly ViecLamDAO _viecLamDAO;
        private readonly NguoiDungDAO _nguoiDungDAO;
        private readonly UngTuyenDAO _ungTuyenDAO;

        public AdminController(ViecLamDAO viecLamDAO, NguoiDungDAO nguoiDungDAO, UngTuyenDAO ungTuyenDAO)
        {
            _viecLamDAO = viecLamDAO;
            _nguoiDungDAO = nguoiDungDAO;
            _ungTuyenDAO = ungTuyenDAO;

        }

        [HttpGet]
        [Route("QuanLyViecLam")]
        [Route("")]
        public async Task<IActionResult> QuanLyViecLam()
        {
            var ndjson = HttpContext.Session.GetString("NguoiDung");

            if (ndjson == null)
            {
                return RedirectToAction("Login", "TaiKhoan");
            }
            var nguoiDung = JsonConvert.DeserializeObject<NguoiDung>(ndjson);
            var quyen = HttpContext.Session.GetString("QuyenHan");
            if (quyen == null || !quyen.Equals("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            var dsViecLam = await _viecLamDAO.GetAll();
            return View("~/Views/Admin/QuanLyViecLam.cshtml",dsViecLam);
        }

        [HttpGet]
        [Route("ThemViecLam")]
        public IActionResult ThemViecLam()
        {
            var ndjson = HttpContext.Session.GetString("NguoiDung");

            if (ndjson == null)
            {
                return RedirectToAction("Login", "TaiKhoan");
            }
            var nguoiDung = JsonConvert.DeserializeObject<NguoiDung>(ndjson);
            var quyen = HttpContext.Session.GetString("QuyenHan");
            if (quyen == null || !quyen.Equals("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View("~/Views/Admin/ThemViecLam.cshtml");
        }


        [HttpPost]
        [Route("ThemViecLam")]
        public async Task<IActionResult> ThemViecLam(ViecLam model)
        {
            // Kiểm tra tính hợp lệ của ModelState
            if (!ModelState.IsValid)
            {
                // Nếu ModelState không hợp lệ, trả về view với model để hiển thị lỗi
                return View(model);
            }

            // Kiểm tra điều kiện Ngày hết hạn phải lớn hơn Ngày tạo
            if (model.NgayHetHan <= model.NgayTao)
            {
                ModelState.AddModelError("NgayHetHan", "Ngày hết hạn phải lớn hơn ngày tạo");
                return View(model);
            }

            // Nếu tất cả điều kiện đều hợp lệ, tạo mới đối tượng ViecLam
            var viecLam = new ViecLam
            {
                TieuDe = model.TieuDe,
                MoTa = model.MoTa,
                TTLienHe = model.TTLienHe,
                MucLuong = model.MucLuong,
                NgayTao = model.NgayTao,
                NgayHetHan = model.NgayHetHan,
                TrangThai = model.TrangThai,//Convert.ToBoolean(model.TrangThai)
                //VerifyKey = model.VerifyKey, 
            };

            // Lưu đối tượng ViecLam vào cơ sở dữ liệu
            await _viecLamDAO.Save(viecLam);

            // Chuyển hướng đến trang quản lý việc làm
            return RedirectToAction("QuanLyViecLam");
        }


        [HttpGet]
        [Route("SuaViecLam")]
        public async Task<IActionResult> Edit(int? id){
            if(id==null){
                return NotFound();
            }
            ViecLam vieclam = await _viecLamDAO.GetByID(id ?? 0);
            return View("SuaViecLam",vieclam);
        }
        [HttpPost]
        [Route("SuaViecLam")]
        public async Task<IActionResult> Edit([Bind("MaViecLam","TieuDe","MoTa","MucLuong","NgayTao","NgayHetHan","TrangThai", "TTLienHe")] ViecLam vieclam){
            if(ModelState.IsValid){
                try{
                    
                    await _viecLamDAO.Update(vieclam);
                }
                catch(Exception ex){
                    if(!ViecLamExits(vieclam.MaViecLam)){
                        return NotFound();
                    }else{
                        throw;
                    }
                }
            return RedirectToAction(nameof(QuanLyViecLam));

            }
            return View("SuaViecLam",vieclam);
        }

        private bool ViecLamExits(int id){
            if(_viecLamDAO.GetByID(id) != null){
                return true;
            }
            return false;
        }
        [HttpGet]
        [Route("QuanLyUngTuyen/{id_vieclam}")]
        public async Task<IActionResult> QuanLyUngTuyen(int id_vieclam)
        {
            List<DonUngTuyen> DSDon = _ungTuyenDAO.getDonByMaViecLam(id_vieclam);
            ViecLam viecLam = await _viecLamDAO.GetByID(id_vieclam);
            List<UngTuyenViewModel> _UngTuyenViews = new List<UngTuyenViewModel>();
            foreach(DonUngTuyen don in DSDon)
            {
                var ungTuyenView = new UngTuyenViewModel
                {
                    HoTen = don.NguoiDung.TenND,
                    Email = don.NguoiDung.Email,
                    SDT = don.NguoiDung.SDT,
                    NgaySinh = don.NguoiDung.NgaySinh,
                    MoTa = don.MoTa,
                    MaND = don.iMaND
                    
                };
                _UngTuyenViews.Add(ungTuyenView);

            }
            QuanLyUngTuyenViewModel model = new QuanLyUngTuyenViewModel {
                TieuDe = viecLam.TieuDe,
                MoTa = viecLam.MoTa,
                MucLuong = viecLam.MucLuong,
                UngTuyenViews = _UngTuyenViews
            };

            return View(model); 
        }


		[HttpGet]
		[Route("ThongBao/{id_nd}")]
		public async Task<IActionResult> ThongBao(int id_nd)
		{
            var ndjson = HttpContext.Session.GetString("NguoiDung");
            if (ndjson == null)
            {
                return RedirectToAction("Login", "TaiKhoan");
            }

            NguoiDung nd  = await _nguoiDungDAO.GetByID(id_nd);
            ThongBaoViewModel model = new ThongBaoViewModel();
            model.ToEmail = nd.Email;
            ViewBag.id_nd = id_nd;
            return View(model);
		}

        [HttpPost]
        [Route("ThongBao/{id_nd}")]
        public async Task<IActionResult> ThongBao(int id_nd,ThongBaoViewModel model)
        {
            ViewBag.id_nd = id_nd;
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int check = await Utils.MailUtils.GuiThongBao(model.ToEmail.Trim(), model.Subject.Trim(), model.Message.Trim());
            ViewBag.MessageCode = check;
            if(check == 1)
            {
                ViewBag.Message = "Gửi thành công";
            }
            else
            {
                ViewBag.Message = "Gửi thất bại";
            }
            return View(model);
        }


        //xóa việc làm 
        [HttpPost]
        [Route("XoaViecLam/{id_vieclam}")]
        public async Task<IActionResult> XoaViecLam(int id_vieclam)
        {
            bool result = await _viecLamDAO.Delete(id_vieclam);
            if (result)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Xóa việc làm không thành công." });
        }





        //them viec lam bang ajax
        //[HttpPost]
        //[Route("ThemViecLam")]
        //public async Task<IActionResult> ThemViecLam(ViecLam model)
        //{
        //    // Kiểm tra tính hợp lệ của ModelState
        //    if (!ModelState.IsValid)
        //    {
        //        return Json(new { success = false, message = "Dữ liệu không hợp lệ", errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) });
        //    }

        //    // Kiểm tra điều kiện Ngày hết hạn phải lớn hơn Ngày tạo
        //    if (model.NgayHetHan <= model.NgayTao)
        //    {
        //        ModelState.AddModelError("NgayHetHan", "Ngày hết hạn phải lớn hơn ngày tạo");
        //        return Json(new { success = false, message = "Ngày hết hạn phải lớn hơn ngày tạo" });
        //    }

        //    // Nếu tất cả điều kiện đều hợp lệ, tạo mới đối tượng ViecLam
        //    var viecLam = new ViecLam
        //    {
        //        TieuDe = model.TieuDe,
        //        MoTa = model.MoTa,
        //        TTLienHe = model.TTLienHe,
        //        MucLuong = model.MucLuong,
        //        NgayTao = model.NgayTao,
        //        NgayHetHan = model.NgayHetHan,
        //        TrangThai = Convert.ToBoolean(model.TrangThai)
        //    };

        //    // Lưu đối tượng ViecLam vào cơ sở dữ liệu
        //    await _viecLamDAO.Save(viecLam);

        //    // Trả về kết quả JSON thành công
        //    return Json(new { success = true, message = "Thêm việc làm thành công" });
        //}

    }
}
