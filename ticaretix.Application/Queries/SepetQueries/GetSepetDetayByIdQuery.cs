﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticaretix.Core.Entities;
using ticaretix.Core.Interfaces;

namespace ticaretix.Application.Queries.SepetQueries
{
    public record GetSepetDetayByIdQuery(int id):IRequest<SepetDetaylariEntity>;
    public class GetSepetDetayByIdQueryHandler(ISepetDetaylarıRepository sepetDetaylarıRepository) : IRequestHandler<GetSepetDetayByIdQuery, SepetDetaylariEntity>
    {
        public async Task<SepetDetaylariEntity> Handle(GetSepetDetayByIdQuery request, CancellationToken cancellationToken)
        {
            return await sepetDetaylarıRepository.GetSepetDetayByIdAsync(request.id);
        }
    }
}
