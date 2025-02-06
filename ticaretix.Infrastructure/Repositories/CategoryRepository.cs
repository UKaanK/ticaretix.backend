using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ticaretix.Core.Entities;
using ticaretix.Core.Interfaces;
using ticaretix.Infrastructure.Data;
using ticaretix.Infrastructure.Redis;

namespace ticaretix.Infrastructure.Repositories
{
    public class CategoryRepository(AppDbContext _dbContext,IRedisService _redisService) : ICategoryRepository
    {
        public async Task<CategoryEntity> AddCategoryAsync(CategoryEntity entity)
        {
            // Kategori adı boş veya null olamaz
            if (string.IsNullOrWhiteSpace(entity.KategoriAdi))
                throw new ArgumentException("Kategori adı boş olamaz!");

            // Aynı isimde kategori var mı kontrolü
            bool exists = await _dbContext.Kategoriler.AnyAsync(x => x.KategoriAdi == entity.KategoriAdi);
            if (exists)
                throw new InvalidOperationException("Bu isimde bir kategori zaten mevcut!");

            await _dbContext.Kategoriler.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            // Yeni kategori eklenince cache'i temizle
            await _redisService.RemoveAsync("categories");

            return entity;
        }

        public async Task<bool> DeleteCategoryAsync(string categoryName)
        {
            // Kategori adı boş mu?
            if (string.IsNullOrWhiteSpace(categoryName))
                throw new ArgumentException("Kategori adı boş olamaz!");

            var kategori = await _dbContext.Kategoriler.FirstOrDefaultAsync(x => x.KategoriAdi == categoryName);
            if (kategori is null)
                throw new KeyNotFoundException("Silinmek istenen kategori bulunamadı!");

            _dbContext.Kategoriler.Remove(kategori);
            bool result = await _dbContext.SaveChangesAsync() > 0;

            if (result)
            {
                // Kategori silindiğinde cache'i temizle
                await _redisService.RemoveAsync("categories");
            }

            return result;
        }

        public async Task<IEnumerable<CategoryEntity>> GetCategory()
        {
            // Kategorileri cache'den almayı dene
            var cachedCategories = await _redisService.GetAsync<List<CategoryEntity>>("categories");
            if (cachedCategories != null)
            {
                return cachedCategories;  // Eğer cache'de varsa, veritabanından sorgulama yapmadan döndür
            }

            // Cache'de yoksa veritabanından al ve cache'e ekle
            var categories = await _dbContext.Kategoriler.ToListAsync();
            await _redisService.SetAsync("categories", categories);
           var result= await _redisService.GetAsync<List<CategoryEntity>>("categories");
            Console.WriteLine(result);
            return categories;
        }

        public async Task<CategoryEntity> GetCategoryByNameAsync(string name)
        {
            // Kategori adı boş mu?
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Kategori adı boş olamaz!");

            // Kategoriyi cache'den almayı dene
            var cachedCategory = await _redisService.GetAsync<CategoryEntity>($"category:{name}");
            if (cachedCategory != null)
            {
                return cachedCategory;  // Eğer cache'de varsa, veritabanından sorgulama yapmadan döndür
            }

            var kategori = await _dbContext.Kategoriler.FirstOrDefaultAsync(x => x.KategoriAdi == name);
            if (kategori is null)
                throw new KeyNotFoundException("Aranan kategori bulunamadı!");

            // Kategoriyi cache'e ekle
            await _redisService.SetAsync($"category:{name}", kategori);
            return kategori;
        }

        public async Task<CategoryEntity> UpdateCategoryAsync(string categoryName, CategoryEntity entity)
        {
            // Kategori adı boş mu?
            if (string.IsNullOrWhiteSpace(categoryName) || string.IsNullOrWhiteSpace(entity.KategoriAdi))
                throw new ArgumentException("Kategori adı boş olamaz!");

            var kategori = await _dbContext.Kategoriler.FirstOrDefaultAsync(x => x.KategoriAdi == categoryName);
            if (kategori is null)
                throw new KeyNotFoundException("Güncellenmek istenen kategori bulunamadı!");

            // Eğer kategori ismi değiştirilmişse, yeni isim daha önce kullanılmış mı kontrol et
            if (categoryName != entity.KategoriAdi)
            {
                bool exists = await _dbContext.Kategoriler.AnyAsync(x => x.KategoriAdi == entity.KategoriAdi);
                if (exists)
                    throw new InvalidOperationException("Bu isimde bir kategori zaten mevcut!");
            }

            kategori.KategoriAdi = entity.KategoriAdi;
            kategori.Aciklama = entity.Aciklama;
            await _dbContext.SaveChangesAsync();

            // Kategori güncellendikten sonra cache'i temizle
            await _redisService.RemoveAsync("categories");
            await _redisService.RemoveAsync($"category:{categoryName}");
            await _redisService.SetAsync($"category:{entity.KategoriAdi}", kategori);

            return kategori;
        }
    }
}
