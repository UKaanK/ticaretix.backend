using Microsoft.EntityFrameworkCore;
using ticaretix.Core.Entities;
using ticaretix.Core.Interfaces;
using ticaretix.Infrastructure.Data;

namespace ticaretix.Infrastructure.Repositories
{
    internal class UrunlerRepository(AppDbContext dbContext) : IUrunlerRepository
    {
        public async Task<UrunlerEntity> AddUrunAsync(UrunlerEntity entity)
        {
            await dbContext.Urunler.AddAsync(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteUruneAsync(int urunId)
        {
              var urun= await dbContext.Urunler.FirstOrDefaultAsync(x=>x.UrunID==urunId);
            if (urun is not null)
            {
                dbContext.Urunler.Remove(urun);
                return await dbContext.SaveChangesAsync()>0;
            }
            return false;
        }

        public async Task<UrunlerEntity> GetUrunByIdAsync(int id)
        {
            return await dbContext.Urunler.FirstOrDefaultAsync(X=>X.UrunID==id);
        }

        public async Task<IEnumerable<UrunlerEntity>> GetUrunler()
        {
            return await dbContext.Urunler.ToListAsync();
        }

        public async Task<UrunlerEntity> UpdateUrunAsync(int urunId, UrunlerEntity entity)
        {
            var urun= await dbContext.Urunler.FirstOrDefaultAsync(x=>x.UrunID==urunId);
            if (urun is not null) {
                urun.StokMiktari=entity.StokMiktari;
                urun.Fiyat=entity.Fiyat;
                urun.KategoriID=entity.KategoriID;
                urun.Aciklama=entity.Aciklama;
                urun.Image=entity.Image;
                urun.UrunAdi=entity.UrunAdi;
                await dbContext.SaveChangesAsync();
                return urun;
            }
            return entity;
        }
        public async Task<IEnumerable<UrunlerEntity>> SearchUrunAsync(string searchTerm, int? categoryId = null)
        {
            var query = dbContext.Urunler.AsQueryable();

            // Eğer kategoriId parametresi verilmişse, kategoriye göre filtrele
            if (categoryId.HasValue)
            {
                query = query.Where(x => x.KategoriID == categoryId.Value);
            }

            // Eğer searchTerm boş değilse, veritabanında LIKE araması yap
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(x => EF.Functions.Like(x.UrunAdi, $"%{searchTerm}%"));
            }

            // Eğer searchTerm boşsa, tüm ürünleri döndür
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query; // Burada herhangi bir ek filtreleme yapmıyoruz, yani tüm ürünleri getiriyoruz
            }

            return await query.ToListAsync();
        }


        public async Task<IEnumerable<UrunlerEntity>> GetUrunlerByCategoryAsync(int categoryId)
        {
            return await dbContext.Urunler
                                  .Where(x => x.KategoriID == categoryId)
                                  .ToListAsync();
        }

    }
}
