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
    public record AddKullaniciCommand(KullaniciEntity Kullanici) : IRequest<KullaniciEntity>;
    public class AddKullaniciCommandHandler : IRequestHandler<AddKullaniciCommand, KullaniciEntity>
    {
        private readonly IKullaniciRepository _kullaniciRepository;

        public AddKullaniciCommandHandler(IKullaniciRepository kullaniciRepository)
        {
            _kullaniciRepository = kullaniciRepository;
        }
        public async Task<KullaniciEntity> Handle(AddKullaniciCommand request, CancellationToken cancellationToken)
        {
            // Kullanıcı eklemesi yapılır
            var kullanici = await _kullaniciRepository.AddKullaniciAsync(request.Kullanici);
            return kullanici;
        }
    }
}
