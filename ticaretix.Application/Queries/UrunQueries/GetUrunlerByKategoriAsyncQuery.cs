using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticaretix.Core.Entities;
using ticaretix.Core.Interfaces;

namespace ticaretix.Application.Queries.UrunQueries
{
    public record GetUrunlerByKategoriAsyncQuery(int categoryId):IRequest<IEnumerable<UrunlerEntity>>;
    public class GetUrunlerByKategoriAsyncQueryHandler(IUrunlerRepository urunlerRepository) : IRequestHandler<GetUrunlerByKategoriAsyncQuery, IEnumerable<UrunlerEntity>>
    {
        public async Task<IEnumerable<UrunlerEntity>> Handle(GetUrunlerByKategoriAsyncQuery request, CancellationToken cancellationToken)
        {
            return await urunlerRepository.GetUrunlerByCategoryAsync(request.categoryId);
        }
    }
}
