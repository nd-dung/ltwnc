using Microsoft.AspNetCore.Mvc;
using Web_TuyenDung.DAO;
using Web_TuyenDung.Models;
using Newtonsoft.Json;
using Web_TuyenDung.Models.ViewModels;

namespace Web_TuyenDung.Controllers
{
    public class TaiKhoanController : Controller
    {

        private readonly NguoiDungDAO _NDdao;
        private readonly TaiKhoanDAO _TaiKhoanDAO;

        public TaiKhoanController(NguoiDungDAO nguoiDungDAO, TaiKhoanDAO taiKhoanDAO)
        {
            _NDdao = nguoiDungDAO;
            _TaiKhoanDAO = taiKhoanDAO;
        }

        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            string email = loginViewModel.Email.Trim();
            string matKhau = loginViewModel.MatKhau.Trim();
            if(email == "" || matKhau == "")
            {
                return ViewBag.Message = "Yêu cầu nhập đầy đủ thông tin!"; 
            }
            var tk = await Authenticate(email, matKhau);
            if (tk != null)
            {
                NguoiDung nd = tk.NguoiDung;
                var ndJson = JsonConvert.SerializeObject(nd, Formatting.None,
                                            new JsonSerializerSettings()
                                            {
                                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                            });
                HttpContext.Session.SetString("NguoiDung", ndJson);
                HttpContext.Session.SetString("QuyenHan", tk.QuyenHan.TenQuyen);
                if (tk.QuyenHan.TenQuyen.Equals("Admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }

                return RedirectToAction("Index", "Home");
            }
            ViewBag.Message = "Tài khoản hoặc mật khẩu không chính xác";
            return View(loginViewModel);
        }

        //xử lý nếu đề yêu cầu mật sai quá 3 lần thì 30 phút sau cho đăng nhập lại
        //private const int MaxFailedAttempts = 3;
        //private const int LockoutDurationMinutes = 30;

        //[HttpPost]
        //public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        //{
        //    string email = loginViewModel.Email.Trim();
        //    string matKhau = loginViewModel.MatKhau.Trim();

        //    if (email == "" || matKhau == "")
        //    {
        //        ViewBag.Message = "Yêu cầu nhập đầy đủ thông tin!";
        //        return View(loginViewModel);
        //    }

        //    // Lấy thông tin đăng nhập thất bại từ session
        //    var failedLoginAttempts = HttpContext.Session.GetInt32("FailedLoginAttempts") ?? 0;
        //    var lastFailedLoginAttempt = HttpContext.Session.GetString("LastFailedLoginAttempt");

        //    if (failedLoginAttempts >= MaxFailedAttempts)
        //    {
        //        if (DateTime.TryParse(lastFailedLoginAttempt, out var lastAttemptTime))
        //        {
        //            var lockoutEndTime = lastAttemptTime.AddMinutes(LockoutDurationMinutes);
        //            if (DateTime.Now < lockoutEndTime)
        //            {
        //                ViewBag.Message = $"Tài khoản của bạn đã bị khóa. Vui lòng thử lại sau {lockoutEndTime - DateTime.Now:mm\\:ss} phút.";
        //                return View(loginViewModel);
        //            }
        //            else
        //            {
        //                // Reset thông tin khi đã qua thời gian khóa
        //                HttpContext.Session.SetInt32("FailedLoginAttempts", 0);
        //                HttpContext.Session.SetString("LastFailedLoginAttempt", null);
        //            }
        //        }
        //    }

        //    var tk = await Authenticate(email, matKhau);
        //    if (tk != null)
        //    {
        //        NguoiDung nd = tk.NguoiDung;
        //        var ndJson = JsonConvert.SerializeObject(nd, Formatting.None,
        //            new JsonSerializerSettings()
        //            {
        //                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //            });
        //        HttpContext.Session.SetString("NguoiDung", ndJson);
        //        HttpContext.Session.SetString("QuyenHan", tk.QuyenHan.TenQuyen);
        //        if (tk.QuyenHan.TenQuyen.Equals("Admin"))
        //        {
        //            return RedirectToAction("Index", "Admin");
        //        }

        //        return RedirectToAction("Index", "Home");
        //    }

        //    // Xử lý khi đăng nhập thất bại
        //    ViewBag.Message = "Tài khoản hoặc mật khẩu không chính xác";
        //    failedLoginAttempts++;
        //    HttpContext.Session.SetInt32("FailedLoginAttempts", failedLoginAttempts);
        //    HttpContext.Session.SetString("LastFailedLoginAttempt", DateTime.Now.ToString());

        //    return View(loginViewModel);
        //}

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            string hoTen = registerViewModel.HoTen.Trim();
            string email = registerViewModel.Email.Trim();
            string matKhau = registerViewModel.MatKhau.Trim();

            Boolean checkEmail = await XacThucEmail(email);

            if (!ModelState.IsValid || !checkEmail)
            {
                ModelState.AddModelError("Email", "Email đã tồn tại");
                return View(registerViewModel);
                ViewBag.Message = "Không hợp lệ";
            }

            TaiKhoan tk = new TaiKhoan
            {
                TenTaiKhoan = registerViewModel.Email,
                MatKhau = registerViewModel.MatKhau,
                iMaQuyen = 2, 
                NguoiDung = new NguoiDung
                {
                    Email = registerViewModel.Email,
                    TenND = registerViewModel.HoTen
                }
                
            };
            var tkCheck = await _TaiKhoanDAO.Save(tk);
            if(tkCheck == null)
            {
                ViewBag.Message = "Không thành công, thử lại sau!";
                return View(registerViewModel);
            }
            return View("Login");

            
           
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public Task<TaiKhoan> Authenticate(string email, string matKhau)
        {
            return _TaiKhoanDAO.getByUserNameAndPassWord(email, matKhau);
            
        }

        [HttpGet]
        public async Task<IActionResult> ThongTinTaiKhoan()
        {
            //Kiểm tra xem có người dùng nào đăng nhập không
            var ndjson = HttpContext.Session.GetString("NguoiDung");
            if (ndjson == null)
            {
                return RedirectToAction("Login", "TaiKhoan");
            }
            // có đăng nhập: lấy ra nguoiDung (ở dạng Json), convert sang thành đối tượng NguoiDung
            var nguoiDung = JsonConvert.DeserializeObject<NguoiDung>(ndjson);

            TaiKhoan taiKhoan = await _TaiKhoanDAO.getByEmail(nguoiDung.Email);

            return View(taiKhoan);
        }

        [HttpPost]
        public async Task<IActionResult> CapNhatTaiKhoan(TaiKhoan taiKhoanNew)
        {
            //Kiểm tra xem có người dùng nào đăng nhập không
            var ndjson = HttpContext.Session.GetString("NguoiDung");
            if (ndjson == null)
            {
                return RedirectToAction("Login", "TaiKhoan");
            }

            //Kiểm tra tài khoản và mật khẩu gửi đến
            if (string.IsNullOrWhiteSpace(taiKhoanNew.TenTaiKhoan))
            {
                ModelState.AddModelError("TenTaiKhoan", "Không được để trống");
            }
			if (string.IsNullOrWhiteSpace(taiKhoanNew.TenTaiKhoan))
			{
				ModelState.AddModelError("MatKhau", "Không được để trống");
			}

            //lấy người dùng ở Session
			var nguoiDung_Session = JsonConvert.DeserializeObject<NguoiDung>(ndjson);

			int check = 0;
            //cập nhật tài khoản
            var taiKhoan = await _TaiKhoanDAO.getByEmail(nguoiDung_Session.Email); // lấy tài khoản cũ lên
            taiKhoan.TenTaiKhoan = taiKhoanNew.TenTaiKhoan.Trim(); // đổi email
            taiKhoan.MatKhau = taiKhoanNew.MatKhau.Trim(); // đổi mật khẩu
			var tk = _TaiKhoanDAO.Update(taiKhoan); // thực hiện cập nhật
			
            if (tk != null)
            {
                // đồng thời cập nhật lại email ở NguoiDung
                var nguoiDung = await _NDdao.GetByID(nguoiDung_Session.MaND);
                nguoiDung.Email = tk.TenTaiKhoan;
                nguoiDung.TaiKhoan = tk;
                var nd = _NDdao.Update(nguoiDung);

                if (nd != null) {
					//cập nhật đối tượng người dùng mới trong Session
					HttpContext.Session.Clear();
					var ndJson_New = JsonConvert.SerializeObject(nd, Formatting.None,
												new JsonSerializerSettings()
												{
													ReferenceLoopHandling = ReferenceLoopHandling.Ignore
												});
					HttpContext.Session.SetString("NguoiDung", ndJson_New);
					HttpContext.Session.SetString("QuyenHan", tk.QuyenHan.TenQuyen);
                    check = 1;
				}
                else
                {
                    check = 0;
                }
            }

			ViewBag.MessageCode = check;
			if (check == 1) {
				ViewBag.Message = "Cập nhật thành công";
				return View("ThongTinTaiKhoan", taiKhoan);
			}

			ViewBag.Message = "Cập nhật thất bại";
			return View("ThongTinTaiKhoan", taiKhoan);

		}


		[HttpGet]
		public IActionResult ThongTinCaNhan()
		{
			//Kiểm tra xem có người dùng nào đăng nhập không
			var ndjson = HttpContext.Session.GetString("NguoiDung");
			if (ndjson == null)
			{
				return RedirectToAction("Login", "TaiKhoan");
			}
			// có đăng nhập: lấy ra nguoiDung (ở dạng Json), convert sang thành đối tượng NguoiDung
			var nguoiDung = JsonConvert.DeserializeObject<NguoiDung>(ndjson);
			return View(nguoiDung);
		}

		[HttpPost]
		public async Task<IActionResult> CapNhatThongTinCaNhan(NguoiDung nguoiDung_New)
		{
			//Kiểm tra xem có người dùng nào đăng nhập không
			var ndjson = HttpContext.Session.GetString("NguoiDung");
			if (ndjson == null)
			{
				return RedirectToAction("Login", "TaiKhoan");
			}
			// có đăng nhập: lấy ra nguoiDung (ở dạng Json), convert sang thành đối tượng NguoiDung
			var nguoiDung = JsonConvert.DeserializeObject<NguoiDung>(ndjson);
            nguoiDung.TenND = nguoiDung_New.TenND;
            nguoiDung.Email = nguoiDung_New.Email;
            nguoiDung.SDT = nguoiDung_New.SDT;
            nguoiDung.NgaySinh = nguoiDung_New.NgaySinh;
            nguoiDung.GioiTinh = nguoiDung_New.GioiTinh;

            int check = 0;
            // Cập nhật
            var nd = _NDdao.Update(nguoiDung);

            if (nd != null) {

				//cập nhật đối tượng người dùng mới trong Session
				HttpContext.Session.Clear();
				var ndJson_New = JsonConvert.SerializeObject(nd, Formatting.None,
											new JsonSerializerSettings()
											{
												ReferenceLoopHandling = ReferenceLoopHandling.Ignore
											});
				HttpContext.Session.SetString("NguoiDung", ndJson_New);

				//Đồng thời cập nhật lại tên tài khoản
				var taiKhoan = await _TaiKhoanDAO.GetByID(nd.iMaTaiKhoan);
                taiKhoan.TenTaiKhoan = nd.Email;
                var tk = _TaiKhoanDAO.Update(taiKhoan);

                if (tk != null) {
                    check = 1;
                }
                else
                {
                    check = 0;
                }

            }

            ViewBag.MessageCode = check;
            if (check == 1) {
                ViewBag.Message = "Cập nhật thành công";
                return View("ThongTinCaNhan", nd);
            }
			ViewBag.Message = "Cập nhật không thành công";
			return View("ThongTinCaNhan", nguoiDung);
		}

		public async Task<Boolean> XacThucEmail(string email)
        {
            TaiKhoan tk = await _TaiKhoanDAO.getByEmail(email);
            if (tk != null)
            {
                return false;
            }
            return true;
        }




    }
}
