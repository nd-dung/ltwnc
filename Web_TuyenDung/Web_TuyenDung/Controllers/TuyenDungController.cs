using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Web_TuyenDung.DAO;
using Web_TuyenDung.Models;
using Web_TuyenDung.Models.ViewModels;

namespace Web_TuyenDung.Controllers
{
    public class TuyenDungController : Controller
    {

        private readonly ViecLamDAO _ViecLamDAO;
        private readonly NguoiDungDAO _NguoiDungDAO;
        private readonly UngTuyenDAO _UngTuyenDAO;

        public TuyenDungController(ViecLamDAO viecLamDAO, NguoiDungDAO nguoiDungDAO, UngTuyenDAO ungTuyenDAO)
        {
            _ViecLamDAO = viecLamDAO;
            _NguoiDungDAO = nguoiDungDAO;
            _UngTuyenDAO = ungTuyenDAO;
        }

        public async Task<IActionResult> Index()
        {
            
            var dsViecLam = await _ViecLamDAO.GetAll();
            return View(dsViecLam);
        }

        [Route("Thi")]
        public async Task<IActionResult> Thi()
        {

            var dsViecLam = await _ViecLamDAO.GetAll();
            return View(dsViecLam);
        }


        [HttpGet]
        [Route("TuyenDung/UngTuyen/{id_vieclam}")]
        public IActionResult UngTuyen(int id_vieclam)
        {
            var NDJson = HttpContext.Session.GetString("NguoiDung");

            if (NDJson == null)
            {
                return RedirectToAction("Login", "TaiKhoan");
            }

            var nguoiDung = JsonConvert.DeserializeObject<NguoiDung>(NDJson);
            UngTuyenViewModel model = new UngTuyenViewModel();
            model.HoTen = nguoiDung.TenND;
            model.Email = nguoiDung.Email;
            model.SDT = nguoiDung.SDT;
            model.NgaySinh = nguoiDung.NgaySinh;
            model.MaND = nguoiDung.MaND;
            model.MaViecLam = id_vieclam;
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> UngTuyen(UngTuyenViewModel model)
        {
            if (!ModelState.IsValid)
            {      
                return View(model);
            }

            NguoiDung nd = await _NguoiDungDAO.GetByID(model.MaND);
            nd.NgaySinh = model.NgaySinh;
            nd.SDT = model.SDT;
            nd.GioiTinh = "nam";
            nd = _NguoiDungDAO.Update(nd);

            var donUT = new DonUngTuyen
            {
                iMaND = model.MaND,
                iMaViecLam = model.MaViecLam,
                ViecLam = await _ViecLamDAO.GetByID(model.MaViecLam),
                NguoiDung = nd,
                MoTa = model.MoTa,
                
            };
            await _UngTuyenDAO.UngTuyen(donUT);

            return RedirectToAction("Index");
        }

        //[HttpGet]
        //public async Task<IActionResult> TimKiem(string search)
        //{
        //    if (string.IsNullOrEmpty(search))
        //    {
        //        return RedirectToAction("Index");
        //    }

        //    var dsViecLam = await _ViecLamDAO.SearchByTitle(search);
        //    return View("Index", dsViecLam);
        //}

        [HttpPost]
        [Route("XoaViecLam/{id_vieclam}")]
        public async Task<IActionResult> XoaViecLam(int id_vieclam)
        {
            bool result = await _ViecLamDAO.Delete(id_vieclam);
            if (result)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Xóa việc làm không thành công." });
        }



    }
}
