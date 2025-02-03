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
    public class CategoryRepository(AppDbContext dbContext) : ICategoryRepository
    {
        public async Task<CategoryEntity> AddCategoryAsync(CategoryEntity entity)
        {
            // Kategori adı boş veya null olamaz
            if (string.IsNullOrWhiteSpace(entity.KategoriAdi))
                throw new ArgumentException("Kategori adı boş olamaz!");

            // Aynı isimde kategori var mı kontrolü
            bool exists = await dbContext.Kategoriler.AnyAsync(x => x.KategoriAdi == entity.KategoriAdi);
            if (exists)
                throw new InvalidOperationException("Bu isimde bir kategori zaten mevcut!");

            await dbContext.Kategoriler.AddAsync(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteCategoryAsync(string categoryName)
        {
            //Kategori adı boş mu?
            if (string.IsNullOrWhiteSpace(categoryName))
                throw new ArgumentException("Kategori adı boş olamaz!");

            var kategori = await dbContext.Kategoriler.FirstOrDefaultAsync(x => x.KategoriAdi == categoryName);
            if (kategori is null)
                throw new KeyNotFoundException("Silinmek istenen kategori bulunamadı!");

            dbContext.Kategoriler.Remove(kategori);
            return await dbContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<CategoryEntity>> GetCategory()
        {
            return await dbContext.Kategoriler.ToListAsync();
        }

        public async Task<CategoryEntity> GetCategoryByNameAsync(string name)
        {
            //Kategori adı boş mu?
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Kategori adı boş olamaz!");

            var kategori = await dbContext.Kategoriler.FirstOrDefaultAsync(x => x.KategoriAdi == name);
            if (kategori is null)
                throw new KeyNotFoundException("Aranan kategori bulunamadı!");

            return kategori;
        }

        public async Task<CategoryEntity> UpdateCategoryAsync(string categoryName, CategoryEntity entity)
        {
            // Kategori adı boş mu?
            if (string.IsNullOrWhiteSpace(categoryName) || string.IsNullOrWhiteSpace(entity.KategoriAdi))
                throw new ArgumentException("Kategori adı boş olamaz!");

            var kategori = await dbContext.Kategoriler.FirstOrDefaultAsync(x => x.KategoriAdi == categoryName);
            if (kategori is null)
                throw new KeyNotFoundException("Güncellenmek istenen kategori bulunamadı!");

            // Eğer kategori ismi değiştirilmişse, yeni isim daha önce kullanılmış mı kontrol et
            if (categoryName != entity.KategoriAdi)
            {
                bool exists = await dbContext.Kategoriler.AnyAsync(x => x.KategoriAdi == entity.KategoriAdi);
                if (exists)
                    throw new InvalidOperationException("Bu isimde bir kategori zaten mevcut!");
            }

            kategori.KategoriAdi = entity.KategoriAdi;
            kategori.Aciklama = entity.Aciklama;
            await dbContext.SaveChangesAsync();
            return kategori;
        }
    }
}
