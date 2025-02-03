using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticaretix.Core.Entities;
using ticaretix.Core.Interfaces;

namespace ticaretix.Application.Queries.KullanıcıQueries
{
    public record GetAllKullaniciQuery() : IRequest<IEnumerable<KullaniciEntity>>;
    public class GetAllKullaniciQueryHandler(IKullaniciRepository kullaniciRepository) : IRequestHandler<GetAllKullaniciQuery, IEnumerable<KullaniciEntity>>
    {
        public async Task<IEnumerable<KullaniciEntity>> Handle(GetAllKullaniciQuery request, CancellationToken cancellationToken)
        {
            return await kullaniciRepository.GetKullanicilar();
        }
    }
}
