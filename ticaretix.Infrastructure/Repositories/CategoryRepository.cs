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
    public class CategoryRepository(AppDbContext dbContext) : ICategoryRepository
    {
        public async Task<CategoryEntity> AddCategoryAsync(CategoryEntity entity)
        {
            await dbContext.Kategoriler.AddAsync(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteCategoryAsync(string categoryName)
        {
            var kategori= await dbContext.Kategoriler.FirstOrDefaultAsync(x=>x.KategoriAdi==categoryName);
            if (kategori is not null)
            {
                dbContext.Kategoriler.Remove(kategori);
                return await dbContext.SaveChangesAsync()>0;
            }
            return false;
        }

        public async Task<IEnumerable<CategoryEntity>> GetCategory()
        {
            return await dbContext.Kategoriler.ToListAsync();
        }

        public async Task<CategoryEntity> GetCategoryByNameAsync(string name)
        {
            return await dbContext.Kategoriler.FirstOrDefaultAsync(x => x.KategoriAdi == name);
        }

        public async Task<CategoryEntity> UpdateCategoryAsync(string categoryName, CategoryEntity entity)
        {
            var kategori = await dbContext.Kategoriler.FirstOrDefaultAsync(x => x.KategoriAdi == categoryName);
            if (kategori is not null) { 
                kategori.KategoriAdi=entity.KategoriAdi;
                kategori.Aciklama=entity.Aciklama;
                await dbContext.SaveChangesAsync();
                return kategori;
            }
            return entity;
        }
    }
}
