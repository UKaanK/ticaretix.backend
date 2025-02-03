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
    public record UpdateUrunCommand(int id,UrunlerEntity urunlerEntity):IRequest<UrunlerEntity>;
    public class UpdateUrunCommandHandler(IUrunlerRepository urunlerRepository) : IRequestHandler<UpdateUrunCommand, UrunlerEntity>
    {
        public async Task<UrunlerEntity> Handle(UpdateUrunCommand request, CancellationToken cancellationToken)
        {
            return await urunlerRepository.UpdateUrunAsync(request.id, request.urunlerEntity);
        }
    }
}
