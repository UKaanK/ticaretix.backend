using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticaretix.Core.Entities;
using ticaretix.Core.Interfaces;

namespace ticaretix.Application.Queries.UrunQueries
{
    public record GetSearchUrunAsyncQuery(string searchTerm, int? categoryId = null):IRequest<IEnumerable<UrunlerEntity>>;
    public class GetSearchUrunAsyncQueryHandler(IUrunlerRepository urunlerRepository) : IRequestHandler<GetSearchUrunAsyncQuery, IEnumerable<UrunlerEntity>>
    {
        public async Task<IEnumerable<UrunlerEntity>> Handle(GetSearchUrunAsyncQuery request, CancellationToken cancellationToken)
        {
            return await urunlerRepository.SearchUrunAsync(request.searchTerm,request?.categoryId);
        }
    }
}
