using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticaretix.Core.Interfaces;

namespace ticaretix.Application.Commands.SepetCommands
{
    public record DeleteSepetUrunCommand(int urunID,int sepetId):IRequest<bool>;
    public class DeleteSepetUrunCommandHandler(ISepetDetaylarıRepository sepetDetaylarıRepository) : IRequestHandler<DeleteSepetUrunCommand, bool>
    {
        public async Task<bool> Handle(DeleteSepetUrunCommand request, CancellationToken cancellationToken)
        {
            return await sepetDetaylarıRepository.DeleteSepetUrunAsync(request.urunID,request.sepetId);
        }
    }
}
