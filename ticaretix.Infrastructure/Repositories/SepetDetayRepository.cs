using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ticaretix.Core.Entities;
using ticaretix.Core.Interfaces;
using ticaretix.Infrastructure.Data;

namespace ticaretix.Infrastructure.Repositories
{
    internal class SepetDetayRepository(AppDbContext dbContext) : ISepetDetaylarıRepository
    {
        public async Task<SepetDetaylariEntity> AddSepetUrunAsync(SepetDetaylariEntity entity)
        {
            //  Ürün ve Sepet geçerli mi?
            if (entity.UrunID <= 0 || entity.SepetID <= 0)
                throw new ArgumentException("Geçersiz ürün veya sepet ID!");

            // 🔴 Aynı ürün sepette var mı?
            var existingProduct = await dbContext.SepetDetaylari
                .FirstOrDefaultAsync(x => x.SepetID == entity.SepetID && x.UrunID == entity.UrunID);

            if (existingProduct != null)
            {
                // Eğer ürün zaten sepette varsa, miktarı artır
                existingProduct.Miktar += entity.Miktar;

                // Güncellenen sepet detayını kaydet
                dbContext.SepetDetaylari.Update(existingProduct);
                await dbContext.SaveChangesAsync();
                return existingProduct;
            }
            else
            {
                // Sepette ürün yoksa, yeni ürünü ekle
                await dbContext.SepetDetaylari.AddAsync(entity);
                await dbContext.SaveChangesAsync();
                return entity;
            }
        }


        public async Task<bool> DeleteSepetUrunAsync(int urunId, int sepetId)
        {
            // Ürün ve sepet ID geçerli mi?
            if (urunId <= 0 || sepetId <= 0)
                throw new ArgumentException("Geçersiz ürün veya sepet ID!");

            var sepetDetay = await dbContext.SepetDetaylari
                .FirstOrDefaultAsync(x => x.UrunID == urunId && x.SepetID == sepetId);

            // 🔴 Ürün ve sepet bulundu mu?
            if (sepetDetay is null)
                throw new KeyNotFoundException("Sepette bu ürün bulunamadı!");

            dbContext.SepetDetaylari.Remove(sepetDetay);
            var result = await dbContext.SaveChangesAsync();

            return result > 0;
        }

        public async Task<IEnumerable<SepetDetaylariEntity>> GetSepetDetay()
        {
            return await dbContext.SepetDetaylari.ToListAsync();
        }

        public async Task<List<SepetDetaylariEntity>> GetSepetDetayByIdAsync(int id)
        {
            // 🔴 SepetID geçerli mi?
            if (id <= 0)
                throw new ArgumentException("Geçersiz sepet ID!");

            var sepetDetaylari = await dbContext.SepetDetaylari
                .Where(x => x.SepetID == id)
                .Include(x => x.Urun) // İlişkili ürünleri de yükle
                .ToListAsync();

            if (sepetDetaylari is null || sepetDetaylari.Count == 0)
                throw new KeyNotFoundException("Bu sepete ait detay bulunamadı!");

            return sepetDetaylari;
        }
    }
}
