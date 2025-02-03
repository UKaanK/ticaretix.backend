using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticaretix.Core.Interfaces;

namespace ticaretix.Application.Commands
{
    public record DeleteKullaniciCommand(string email) : IRequest<bool>;
    public class DeleteKullaniciCommandHandler(IKullaniciRepository kullaniciRepository)
        : IRequestHandler<DeleteKullaniciCommand, bool>
    {
        public async Task<bool> Handle(DeleteKullaniciCommand request, CancellationToken cancellationToken)
        {
            return await kullaniciRepository.DeleteKullaniciAsync(request.email);
        }
    }
}
