using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticaretix.Core.Entities;
using ticaretix.Core.Interfaces;

namespace ticaretix.Application.Queries
{
    public record GetKullaniciByIdQuery(string email):IRequest<KullaniciEntity>;
    public class GetKullaniciByIdQueryHandler(IKullaniciRepository kullaniciRepository)
           : IRequestHandler<GetKullaniciByIdQuery, KullaniciEntity>
    {
        public async Task<KullaniciEntity> Handle(GetKullaniciByIdQuery request, CancellationToken cancellationToken)
        {
            return await kullaniciRepository.GetKullaniciByEmailAsync(request.email);
        }
    }
}
