using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticaretix.Core.Entities;
using ticaretix.Core.Interfaces;

namespace ticaretix.Application.Commands.KullaniciCommands
{
    public record UpdateKullaniciCommand(string email, KullaniciEntity KullaniciEntity)
        : IRequest<KullaniciEntity>;
    public class UpdateKullaniciCommandHandler(IKullaniciRepository kullaniciRepository)
        : IRequestHandler<UpdateKullaniciCommand, KullaniciEntity>
    {
        public async Task<KullaniciEntity> Handle(UpdateKullaniciCommand request, CancellationToken cancellationToken)
        {
            return await kullaniciRepository.UpdateKullaniciAsync(request.email, request.KullaniciEntity);
        }
    }
}
