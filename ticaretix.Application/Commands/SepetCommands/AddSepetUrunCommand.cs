using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticaretix.Core.Entities;
using ticaretix.Core.Interfaces;

namespace ticaretix.Application.Commands.SepetCommands
{
    public record AddSepetUrunCommand(SepetDetaylariEntity sepetDetaylari):IRequest<SepetDetaylariEntity>;
    public class AddSepetUrunCommandHandler : IRequestHandler<AddSepetUrunCommand, SepetDetaylariEntity>
    {
        private readonly ISepetDetaylarıRepository _sepetDetaylariRepository;
        public AddSepetUrunCommandHandler(ISepetDetaylarıRepository sepetDetaylarıRepository)
        {
            _sepetDetaylariRepository = sepetDetaylarıRepository;
        }
        public Task<SepetDetaylariEntity> Handle(AddSepetUrunCommand request, CancellationToken cancellationToken)
        {
            var sepetdetaylari = _sepetDetaylariRepository.AddSepetUrunAsync(request.sepetDetaylari);
            return sepetdetaylari;
        }
    }
}
