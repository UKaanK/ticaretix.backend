using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticaretix.Core.Entities;

namespace ticaretix.Core.Interfaces
{
    public interface IKullaniciRepository
    {
        Task<IEnumerable<KullaniciEntity>> GetKullanicilar();
        Task<KullaniciEntity> GetKullaniciByEmailAsync(string email);
        Task<KullaniciEntity> AddKullaniciAsync(KullaniciEntity entity);
        Task<KullaniciEntity> UpdateKullaniciAsync(string email, KullaniciEntity entity);
        Task<bool> DeleteKullaniciAsync(string email);
    }
}
