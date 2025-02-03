using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticaretix.Core.Entities;
using ticaretix.Core.Interfaces;

namespace ticaretix.Application.Commands.KategoriCommands
{
    public record UpdateKategoriCommand(string categoryname,CategoryEntity categoryEntity):IRequest<CategoryEntity>;
    internal class UpdateKategoriCommandHandler(ICategoryRepository categoryRepository) : IRequestHandler<UpdateKategoriCommand, CategoryEntity>
    {
        public async Task<CategoryEntity> Handle(UpdateKategoriCommand request, CancellationToken cancellationToken)
        {
            return await categoryRepository.UpdateCategoryAsync(request.categoryname, request.categoryEntity);
        }
    }
}
