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
    internal class SepetDetayRepository(AppDbContext dbContext) : ISepetDetaylarıRepository
    {
        public async Task<SepetDetaylariEntity> AddSepetUrunAsync(SepetDetaylariEntity entity)
        {
            await dbContext.SepetDetaylari.AddAsync(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }


        public async Task<bool> DeleteSepetUrunAsync(int urunId,int sepetId)
        {
            var sepet = await dbContext.SepetDetaylari.FirstOrDefaultAsync(x => x.UrunID == urunId);
            if (sepet is not null&& sepet.SepetID==sepetId)
            {
                dbContext.SepetDetaylari.Remove(sepet);
                return await dbContext.SaveChangesAsync()>0;
            }
            return false;
        }

        public async Task<IEnumerable<SepetDetaylariEntity>> GetSepetDetay()
        {
            return await dbContext.SepetDetaylari.ToListAsync();
        }

        public async Task<SepetDetaylariEntity> GetSepetDetayByIdAsync(int id)
        {
            return  await dbContext.SepetDetaylari.FirstOrDefaultAsync(x => x.SepetID == id);
           
        }
    }
}
