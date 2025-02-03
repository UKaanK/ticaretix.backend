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
    public record AddUrunCommand(UrunlerEntity urunEntity):IRequest<UrunlerEntity>;
    public class AddUrunCommandHandler : IRequestHandler<AddUrunCommand, UrunlerEntity>
    {
        private readonly IUrunlerRepository _urunlerRepository;
        public AddUrunCommandHandler(IUrunlerRepository urunlerRepository)
        {
            _urunlerRepository = urunlerRepository;
        }
        public async Task<UrunlerEntity> Handle(AddUrunCommand request, CancellationToken cancellationToken)
        {
            var urunler = await _urunlerRepository.AddUrunAsync(request.urunEntity);
            return urunler;
        }
    }
}
