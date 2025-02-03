using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            // 🔴 Email ve şifre boş olamaz
            if (string.IsNullOrWhiteSpace(entity.Email) || string.IsNullOrWhiteSpace(entity.Sifre))
                throw new ArgumentException("Email ve şifre boş olamaz!");

            // 🔴 Email formatı geçerli mi?
            if (!Regex.IsMatch(entity.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("Geçersiz email formatı!");

            // 🔴 Aynı email ile kayıtlı kullanıcı var mı?
            bool exists = await dbContext.Kullanicilar.AnyAsync(x => x.Email == entity.Email);
            if (exists)
                throw new InvalidOperationException("Bu e-posta adresi zaten kayıtlı!");

            // 🔴 Şifre uzunluğu kontrolü
            if (entity.Sifre.Length < 6)
                throw new ArgumentException("Şifre en az 6 karakter olmalıdır!");

            await dbContext.Kullanicilar.AddAsync(entity);
            await dbContext.SaveChangesAsync();

            // 🔴 Kullanıcı için otomatik sepet oluştur
            var yeniSepet = new SepetEntity
            {
                KullaniciID = entity.KullaniciID,
                OlusturmaTarihi = DateTime.Now
            };

            await dbContext.Sepetler.AddAsync(yeniSepet);
            var result = await dbContext.SaveChangesAsync();

            // 🔴 Eğer sepet oluşturulamazsa hata fırlat
            if (result == 0)
                throw new InvalidOperationException("Sepet oluşturulurken hata oluştu!");

            return entity;
        }

        public async Task<bool> DeleteKullaniciAsync(string email)
        {
            // 🔴 Email boş mu?
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email boş olamaz!");

            var kullanici = await dbContext.Kullanicilar.FirstOrDefaultAsync(x => x.Email == email);
            if (kullanici is null)
                throw new KeyNotFoundException("Silinmek istenen kullanıcı bulunamadı!");

            // 🔴 Kullanıcıya ait sepeti de silelim
            var kullaniciSepeti = await dbContext.Sepetler.FirstOrDefaultAsync(x => x.KullaniciID == kullanici.KullaniciID);
            if (kullaniciSepeti is not null)
            {
                dbContext.Sepetler.Remove(kullaniciSepeti);
            }

            dbContext.Kullanicilar.Remove(kullanici);
            return await dbContext.SaveChangesAsync() > 0;
        }

        public async Task<KullaniciEntity> GetKullaniciByEmailAsync(string email)
        {
            // 🔴 Email boş mu?
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email boş olamaz!");

            var kullanici = await dbContext.Kullanicilar.FirstOrDefaultAsync(x => x.Email == email);
            if (kullanici is null)
                throw new KeyNotFoundException("Kullanıcı bulunamadı!");

            return kullanici;
        }

        public async Task<IEnumerable<KullaniciEntity>> GetKullanicilar()
        {
            return await dbContext.Kullanicilar.ToListAsync();
        }

        public async Task<KullaniciEntity> UpdateKullaniciAsync(string email, KullaniciEntity entity)
        {
            // 🔴 Email boş mu?
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(entity.Email))
                throw new ArgumentException("Email boş olamaz!");

            var kullanici = await dbContext.Kullanicilar.FirstOrDefaultAsync(x => x.Email == email);
            if (kullanici is null)
                throw new KeyNotFoundException("Güncellenmek istenen kullanıcı bulunamadı!");

            // 🔴 Eğer email değiştiriliyorsa, yeni email başka kullanıcıya ait mi kontrolü
            if (email != entity.Email)
            {
                bool emailExists = await dbContext.Kullanicilar.AnyAsync(x => x.Email == entity.Email);
                if (emailExists)
                    throw new InvalidOperationException("Bu e-posta adresi zaten başka bir kullanıcı tarafından kullanılıyor!");
            }

            // 🔴 Şifre uzunluğu kontrolü
            if (!string.IsNullOrEmpty(entity.Sifre) && entity.Sifre.Length < 6)
                throw new ArgumentException("Şifre en az 6 karakter olmalıdır!");

            kullanici.KullaniciAdi = entity.KullaniciAdi;
            kullanici.Sifre = entity.Sifre;
            kullanici.Role = entity.Role;
            kullanici.Email = entity.Email;

            await dbContext.SaveChangesAsync();
            return kullanici;
        }
    }
}
