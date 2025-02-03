using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticaretix.Core.Entities;

namespace ticaretix.Core.Interfaces
{
    public interface ISepetDetaylarıRepository
    {
        Task<IEnumerable<SepetDetaylariEntity>> GetSepetDetay();
        Task<SepetDetaylariEntity> GetSepetDetayByIdAsync(int id);
        Task<SepetDetaylariEntity> AddSepetUrunAsync(SepetDetaylariEntity entity);
        //Task<SepetDetaylariEntity> UpdateUrunAsync(int urunId, SepetDetaylariEntity entity);
        Task<bool> DeleteSepetUrunAsync(int urunId, int sepetId);
    }
}
