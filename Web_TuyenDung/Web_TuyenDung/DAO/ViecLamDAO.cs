using Microsoft.EntityFrameworkCore;
using Web_TuyenDung.Models;

namespace Web_TuyenDung.DAO
{
    public class ViecLamDAO
    {
        private readonly DataContext _dataContext;

        public ViecLamDAO(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<ViecLam>> GetAll()
        {
            return await _dataContext.DSViecLam
                .Include(vl => vl.DSDonUT)
                .ToListAsync<ViecLam>();
        }

        public async Task<ViecLam> Save(ViecLam viecLam)
        {
            var vl = await _dataContext.DSViecLam.AddAsync(viecLam);
            await _dataContext.SaveChangesAsync();
            return vl.Entity;
        }

        public async Task<ViecLam> GetByID(int id)
        {
            return await _dataContext.DSViecLam.FindAsync(id);
            
        }
        public async Task<ViecLam> Update(ViecLam viecLam)
        {
            var checkID = await GetByID(viecLam.MaViecLam);
            if(checkID!=null)
            {
                
                checkID.TieuDe= viecLam.TieuDe;
                checkID.MoTa=viecLam.MoTa;
                checkID.NgayHetHan =viecLam.NgayHetHan;
                checkID.MucLuong=viecLam.MucLuong;
                checkID.NgayTao=viecLam.NgayTao;
                checkID.TrangThai=viecLam.TrangThai;
                checkID.TTLienHe = viecLam.TTLienHe;

                await _dataContext.SaveChangesAsync();
            }
            return viecLam;
        }
         public async Task<bool> Delete(int id)
         {
            var viecLam = await _dataContext.DSViecLam.FindAsync(id);
            if (viecLam == null)
            {
                return false;
            }

            _dataContext.DSViecLam.Remove(viecLam);
            await _dataContext.SaveChangesAsync();
            return true;
         }
         
         public async Task<List<ViecLam>> SearchByTitle(string title)
        {
            return await _dataContext.DSViecLam
                .Where(vl => vl.TieuDe.Contains(title))
                .Include(vl => vl.DSDonUT)
                .ToListAsync();
        }

    }
}
