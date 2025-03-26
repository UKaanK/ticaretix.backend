using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticaretix.Core.Entities;
using ticaretix.Core.Interfaces;

namespace ticaretix.Application.Queries.SepetQueries
{

    public record GetAllSepetDetayQuery():IRequest<IEnumerable<SepetDetaylariEntity>>;
    public class GetAllSepetDetayQueryHandler(ISepetDetaylarıRepository sepetDetaylarıRepository) : IRequestHandler<GetAllSepetDetayQuery, IEnumerable<SepetDetaylariEntity>>
    {
        public async Task<IEnumerable<SepetDetaylariEntity>> Handle(GetAllSepetDetayQuery request, CancellationToken cancellationToken)
        {
            return await sepetDetaylarıRepository.GetSepetDetay();
        }
    }
}
