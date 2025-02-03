using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticaretix.Core.Entities;

namespace ticaretix.Core.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategoryEntity>> GetCategory();
        Task<CategoryEntity> GetCategoryByNameAsync(string name);
        Task<CategoryEntity> AddCategoryAsync(CategoryEntity entity);
        Task<CategoryEntity> UpdateCategoryAsync(string categoryName, CategoryEntity entity);
        Task<bool> DeleteCategoryAsync(string categoryName);
    }
}
