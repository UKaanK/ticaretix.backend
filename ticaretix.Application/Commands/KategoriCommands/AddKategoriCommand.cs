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
    public record AddKategoriCommand(CategoryEntity Entity):IRequest<CategoryEntity>;
    public class AddKategoriCommandHandler : IRequestHandler<AddKategoriCommand, CategoryEntity>
    {
        private readonly ICategoryRepository _categoryRepository;

        public AddKategoriCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<CategoryEntity> Handle(AddKategoriCommand request, CancellationToken cancellationToken)
        {
            var kategori=await _categoryRepository.AddCategoryAsync(request.Entity);
            return kategori;
        }
    }
}
