using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticaretix.Core.Entities;

namespace ticaretix.Core.Interfaces
{
    public interface IUrunlerRepository
    {
        Task<IEnumerable<UrunlerEntity>> GetUrunler();
        Task<UrunlerEntity> GetUrunByIdAsync(int id);
        Task<UrunlerEntity> AddUrunAsync(UrunlerEntity entity);
        Task<UrunlerEntity> UpdateUrunAsync(int urunId, UrunlerEntity entity);
        Task<bool> DeleteUruneAsync(int urunId);
        Task<IEnumerable<UrunlerEntity>> SearchUrunAsync(string searchTerm, int? categoryId = null);
        Task<IEnumerable<UrunlerEntity>> GetUrunlerByCategoryAsync(int categoryId);

    }
}
