using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticaretix.Core.Entities;
using ticaretix.Core.Interfaces;

namespace ticaretix.Application.Queries.KategoriQueries
{
    public record GetKategoriByNameQuery(string name):IRequest<CategoryEntity>;
    public class GetKategoriByNameQueryHandler(ICategoryRepository categoryRepository) : IRequestHandler<GetKategoriByNameQuery, CategoryEntity>
    {
        public async Task<CategoryEntity> Handle(GetKategoriByNameQuery request, CancellationToken cancellationToken)
        {
            return await categoryRepository.GetCategoryByNameAsync(request.name);
        }
    }
}
