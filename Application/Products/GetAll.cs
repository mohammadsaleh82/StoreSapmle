using Application.Core;
using Application.Dtos.Products;
using AutoMapper;
using Domain.Entities.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Products;

public class GetAll
{
    public class Query : IRequest<Result<List<Product>>>
    {
    }

    public class Handler : IRequestHandler<Query, Result<List<Product>>>
    {
        private readonly StoreContext _context;
        public Handler(StoreContext context, IMapper mapper)
        {
            _context = context;
            
        }

        public async Task<Result<List<Product>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var products = await _context.Products.ToListAsync();
            return Result<List<Product>>.Success(products);
        }
    }
}