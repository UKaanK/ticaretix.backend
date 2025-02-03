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
    public record GetUrunByIdQuery(int id):IRequest<UrunlerEntity>;
    public class GetUrunByIdQueryHandler(IUrunlerRepository urunlerRepository) : IRequestHandler<GetUrunByIdQuery, UrunlerEntity>
    {
        public async Task<UrunlerEntity> Handle(GetUrunByIdQuery request, CancellationToken cancellationToken)
        {
            return await urunlerRepository.GetUrunByIdAsync(request.id);
        }
    }
}
