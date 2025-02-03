using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticaretix.Core.Interfaces;

namespace ticaretix.Application.Commands.KategoriCommands
{
    public record DeleteKategoriCommand(string categoryname):IRequest<bool>;
    internal class DeleteKategoriCommandHandler(ICategoryRepository categoryRepository) : IRequestHandler<DeleteKategoriCommand, bool>
    {
        public async Task<bool> Handle(DeleteKategoriCommand request, CancellationToken cancellationToken)
        {
            return await categoryRepository.DeleteCategoryAsync(request.categoryname);
        }
    }
}
