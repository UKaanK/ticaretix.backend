using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticaretix.Core.Entities;
using ticaretix.Core.Interfaces;
using ticaretix.Infrastructure.Data;

namespace ticaretix.Infrastructure.Repositories
{
    internal class KullaniciRepository(AppDbContext dbContext) : IKullaniciRepository
    {
        public async Task<KullaniciEntity> AddKullaniciAsync(KullaniciEntity entity)
        {
            await dbContext.Kullanicilar.AddAsync(entity);
            await dbContext.SaveChangesAsync();

            // Kullanıcıya ait bir sepet otomatik olarak oluşturalım
            var yeniSepet = new SepetEntity
            {
                KullaniciID = entity.KullaniciID, // Kullanıcı ID'sini alıyoruz
                OlusturmaTarihi = DateTime.Now
            };

            await dbContext.Sepetler.AddAsync(yeniSepet);
            await dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteKullaniciAsync(string emaik)
        {
            var kullanici = await dbContext.Kullanicilar.FirstOrDefaultAsync(x => x.Email == emaik);
            if (kullanici is not null) {
                dbContext.Kullanicilar.Remove(kullanici);
                return await dbContext.SaveChangesAsync()>0;
            }
            return false;
        
        }

        public async Task<KullaniciEntity> GetKullaniciByEmailAsync(string email)
        {
            return await dbContext.Kullanicilar.FirstOrDefaultAsync(x => x.Email == email);
        }


        public async Task<IEnumerable<KullaniciEntity>> GetKullanicilar()
        {
           return await dbContext.Kullanicilar.ToListAsync();
        }

        public async Task<KullaniciEntity> UpdateKullaniciAsync(string email, KullaniciEntity entity)
        {
            var kullanici =await dbContext.Kullanicilar.FirstOrDefaultAsync(x=>x.Email == email);
            if (kullanici is not null)
            {
                kullanici.KullaniciAdi = entity.KullaniciAdi;
                kullanici.Sifre = entity.Sifre;
                kullanici.Role = entity.Role;
                kullanici.Email = email;
                await dbContext.SaveChangesAsync();
                return kullanici;
            }
            return entity;
        }
    }
}
