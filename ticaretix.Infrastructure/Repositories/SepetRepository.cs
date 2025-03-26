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
    public class SepetRepository(AppDbContext dbContext) : ISepetRepository
    {
        public async Task<bool> DeleteSepetAsync(int sepetId)
        {
            var sepet = await dbContext.Sepetler.FirstOrDefaultAsync(x => x.SepetID==sepetId);
            if (sepet is not null)
            {
                dbContext.Sepetler.Remove(sepet);
                return await dbContext.SaveChangesAsync()>0;
            }
            return false;
        }

        public async Task<IEnumerable<SepetEntity>> GetSepet()
        {

            return await dbContext.Sepetler.ToListAsync();
        }

    }
}
