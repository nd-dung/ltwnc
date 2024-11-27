using Microsoft.EntityFrameworkCore;
using Web_TuyenDung.Models;

namespace Web_TuyenDung.DAO
{
    public class UngTuyenDAO
    {
        DataContext _dataContext;
        public UngTuyenDAO(DataContext dataContext)
        {
            _dataContext = dataContext;
        } 
        
        public async Task<DonUngTuyen> UngTuyen(DonUngTuyen donUT)
        {
            var don = await _dataContext.DSDonUT.AddAsync(donUT);
            _dataContext.SaveChanges();
            return don.Entity;
        }
        
        public List<DonUngTuyen> getDonByMaViecLam(int maViecLam)
        {
            var list = _dataContext.DSDonUT
            .Include(d => d.NguoiDung)
            .Where(d => d.iMaViecLam == maViecLam)
            .ToList();
            return list;
        }
        
    }
}
