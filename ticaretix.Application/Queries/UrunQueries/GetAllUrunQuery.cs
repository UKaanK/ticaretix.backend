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
    public record GetAllUrunQuery():IRequest<IEnumerable<UrunlerEntity>>;
    public class GetAllUrunQueryHandler(IUrunlerRepository urunlerRepository) : IRequestHandler<GetAllUrunQuery, IEnumerable<UrunlerEntity>>
    {
        public async Task<IEnumerable<UrunlerEntity>> Handle(GetAllUrunQuery request, CancellationToken cancellationToken)
        {
            return await urunlerRepository.GetUrunler();
        }
    }
}
