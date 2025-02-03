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
    public record GetAllSepetQuery():IRequest<IEnumerable<SepetEntity>>;
    internal class GetAllSepetQueryHandler(ISepetRepository sepetRepository)
            : IRequestHandler<GetAllSepetQuery, IEnumerable<SepetEntity>>
    {
        public async Task<IEnumerable<SepetEntity>> Handle(GetAllSepetQuery request, CancellationToken cancellationToken)
        {
            return await sepetRepository.GetSepet();
        }
    }
}
