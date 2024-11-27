using Microsoft.EntityFrameworkCore;
using Web_TuyenDung.Models;

namespace Web_TuyenDung.DAO
{
    public class TaiKhoanDAO
    {
        private readonly DataContext _dataContext;

        public TaiKhoanDAO(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        public async Task<TaiKhoan> getByUserNameAndPassWord(string taiKhoan, string matKhau)
        {
            var tk = await _dataContext.DSTaiKhoan
                                        .Include(t => t.NguoiDung)
                                        .Include(t => t.QuyenHan)
                                        .FirstOrDefaultAsync(t => t.TenTaiKhoan == taiKhoan && t.MatKhau == matKhau);

            return tk;
        }

        public async Task<TaiKhoan> Save(TaiKhoan taiKhoan)
        {
            var tk = await _dataContext.DSTaiKhoan.AddAsync(taiKhoan);
            await _dataContext.SaveChangesAsync();
            
            
                // Đối tượng TaiKhoan đã được thêm và lưu xuống cơ sở dữ liệu
                return tk.Entity;
            
            
        }

        public async Task<TaiKhoan> getByEmail(string email)
        {
            var tk = await _dataContext.DSTaiKhoan
										.Include(t => t.NguoiDung)
                                        .Include(t => t.QuyenHan)
										.FirstOrDefaultAsync(t => t.TenTaiKhoan == email);

            return tk;
        }

        public TaiKhoan Update(TaiKhoan taiKhoan)
        {
            var tk = _dataContext.DSTaiKhoan.Update(taiKhoan);
            _dataContext.SaveChanges();
            return tk.Entity;
        }

		public async Task<TaiKhoan> GetByID(int id)
		{
			return await _dataContext.DSTaiKhoan
                            .Include(t => t.NguoiDung)
                            .Include(t => t.QuyenHan)
							.FirstOrDefaultAsync(t => t.MaTaiKhoan == id);

		}

	}
}
