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
    public record GetAllKategoriQuery():IRequest<IEnumerable<CategoryEntity>>;
    internal class GetAllKategoriQueryHandler(ICategoryRepository categoryRepository) : IRequestHandler<GetAllKategoriQuery, IEnumerable<CategoryEntity>>
    {
        public async Task<IEnumerable<CategoryEntity>> Handle(GetAllKategoriQuery request, CancellationToken cancellationToken)
        {
            return await categoryRepository.GetCategory();
        }
    }
}
