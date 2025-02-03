using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticaretix.Core.Entities;

namespace ticaretix.Core.Interfaces
{
    public interface ISepetRepository
    {
        Task<IEnumerable<SepetEntity>> GetSepet();
      
        Task<bool> DeleteSepetAsync(int sepetId);
    }
}
