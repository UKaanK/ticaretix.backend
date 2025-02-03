using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticaretix.Core.Entities;
using ticaretix.Core.Interfaces;

namespace ticaretix.Application.Commands.UrunCommands
{
    public record DeleteUrunCommand(int id):IRequest<bool>;
    public class DeleteUrunCommandHandler(IUrunlerRepository urunlerRepository) : IRequestHandler<DeleteUrunCommand, bool>
    {
        public async Task<bool> Handle(DeleteUrunCommand request, CancellationToken cancellationToken)
        {
            return await urunlerRepository.DeleteUruneAsync(request.id);
        }
    }
}
